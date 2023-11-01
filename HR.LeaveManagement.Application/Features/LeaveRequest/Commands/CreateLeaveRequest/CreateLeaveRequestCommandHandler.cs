using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
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

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IAppLogger<CreateLeaveRequestCommandHandler> _appLogger;
        private readonly IUserService _userService;

        public CreateLeaveRequestCommandHandler(
            IMapper mapper, 
            IEmailSender emailSender, 
            ILeaveTypeRepository leaveTypeRepository,
            ILeaveAllocationRepository leaveAllocationRepository,
            ILeaveRequestRepository leaveRequestRepository,
            IAppLogger<CreateLeaveRequestCommandHandler> appLogger,
            IUserService userService)
        {
            this._mapper = mapper;
            this._emailSender = emailSender;
            this._leaveTypeRepository = leaveTypeRepository;
            this._leaveRequestRepository = leaveRequestRepository;
            this._appLogger = appLogger;
            this._userService = userService;
            this._leaveAllocationRepository = leaveAllocationRepository;
        }

        public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any()) 
            {
                throw new BadRequestException("Invalid Leave Request", validationResult); 
            }

            // Get requesting employee's Id

            var employeeId = _userService.UserId;

            // Check on Employee's allocation
            var allocation = await _leaveAllocationRepository.GetUserAllocations(employeeId, request.LeaveTypeId);

            // if allocations aren't enough, return validation error with message
            if (allocation is null)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.LeaveTypeId),
                    "You do not have any allocations for this leave type."));
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }

            int daysRequested = (int)(request.EndDate - request.StartDate).TotalDays;
            if (daysRequested > allocation.NumberOfDays)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(
                    nameof(request.EndDate), "You do not have enough days for this request"));
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }

            // Create leave request
            var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
            leaveRequest.RequestingEmployeeId = employeeId;
            leaveRequest.DateRequested = DateTime.Now;

            try
            {
                await _leaveRequestRepository.CreateAsync(leaveRequest);

                // Send confirmation Email
                var email = new EmailMessage
                {
                    To = string.Empty,
                    Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} has been submitted successfully!",
                    Subject = "Leave Request Subbmitted"
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
