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

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger<CancelLeaveRequestCommandHandler> _appLogger;

        public CancelLeaveRequestCommandHandler(
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveAllocationRepository leaveAllocationRepository,
            IEmailSender emailSender, 
            IAppLogger<CancelLeaveRequestCommandHandler> appLogger)
        {
            this._leaveRequestRepository = leaveRequestRepository;
            this._leaveAllocationRepository = leaveAllocationRepository;
            this._emailSender = emailSender;
            this._appLogger = appLogger;
        }
        public async Task<Unit> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

            if (leaveRequest is null) 
            {
                throw new NotFoundException(nameof(leaveRequest), request.Id);
            }

            leaveRequest.Cancelled = true;

            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            //If already approved, Re-evaluate the employee's allocations for the leave type
            
            if(leaveRequest.Approved == true)
            {
                int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                var allocation = await _leaveAllocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);
                allocation.NumberOfDays += daysRequested;

                await _leaveAllocationRepository.UpdateAsync(allocation);
            }

            // Send confirmation email
            try
            {
                var email = new EmailMessage
                {
                    To = string.Empty,
                    Body = $"Your leave request for {leaveRequest.StartDate} to {leaveRequest.EndDate} has been cancelled successfully!",
                    Subject = "Leave Request Cancelled"
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
