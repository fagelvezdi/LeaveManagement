using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<UpdateLeaveRequestCommandHandler> _appLogger;

        public UpdateLeaveRequestCommandHandler(
            IMapper mapper, 
            IEmailSender emailSender, 
            ILeaveRequestRepository leaveRequestRepository, 
            ILeaveTypeRepository leaveTypeRepository,
            IAppLogger<UpdateLeaveRequestCommandHandler> appLogger)
        {
            this._mapper = mapper;
            this._emailSender = emailSender;
            this._leaveRequestRepository = leaveRequestRepository;
            this._leaveTypeRepository = leaveTypeRepository;
            this._appLogger = appLogger;
        }

        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            // Get leave request
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

            //Validate if was found
            if(leaveRequest is null) 
            { 
                throw new NotFoundException(nameof(leaveRequest), request.Id);
            }

            // Implement validator
            var validator = new UpdateLeaveRequestCommandValidator(_leaveTypeRepository, _leaveRequestRepository);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if(validationResult.Errors.Any()) 
            {
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }

            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            _mapper.Map(request, leaveRequest);

            try
            {
                // Send email confirmation
                var email = new EmailMessage
                {
                    To = string.Empty, /*Get email from employee record*/
                    Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} has been updated successfully.",
                    Subject = "Leave Request Updated"
                };

                await _emailSender.SendEmail(email);
            }
            catch (Exception ex)
            {
                _appLogger.LogWarning(ex.Message);
            }

            return Unit.Value;
        }
    }
}
