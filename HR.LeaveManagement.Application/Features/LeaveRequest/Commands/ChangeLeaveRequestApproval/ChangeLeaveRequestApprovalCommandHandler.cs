using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval
{
    public class ChangeLeaveRequestApprovalCommandHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IAppLogger<ChangeLeaveRequestApprovalCommandHandler> _appLogger;

        public ChangeLeaveRequestApprovalCommandHandler(
            IMapper mapper,
            IEmailSender emailSender,
            ILeaveTypeRepository leaveTypeRepository,
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveAllocationRepository leaveAllocationRepository,
            IAppLogger<ChangeLeaveRequestApprovalCommandHandler> appLogger)
        {
            this._mapper = mapper;
            this._emailSender = emailSender;
            this._leaveTypeRepository = leaveTypeRepository;
            this._leaveRequestRepository = leaveRequestRepository;
            this._appLogger = appLogger;
            this._leaveAllocationRepository = leaveAllocationRepository;
        }

        public async Task<Unit> Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

            if(leaveRequest is null)
            {
                throw new NotFoundException(nameof(leaveRequest), request.Id);
            }

            leaveRequest.Approved = request.Approved;
            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            // If request is approved, get and update the employee's allocation
            if(request.Approved)
            {
                int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                var allocation = await _leaveAllocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);
                allocation.NumberOfDays -= daysRequested;

                await _leaveAllocationRepository.UpdateAsync(allocation);
            }

            try
            {
                // Send confirmation email
                var email = new EmailMessage
                {
                    To = string.Empty,
                    Body = $"The approval status for your leave request for {leaveRequest.StartDate} to {leaveRequest.EndDate} has been updated!",
                    Subject = "Leave Request Approval Updated"
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
