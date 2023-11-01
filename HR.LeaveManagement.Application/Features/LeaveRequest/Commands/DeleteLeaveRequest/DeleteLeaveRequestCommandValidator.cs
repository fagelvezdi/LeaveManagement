using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest
{
    public class DeleteLeaveRequestCommandValidator : AbstractValidator<DeleteLeaveRequestCommand>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public DeleteLeaveRequestCommandValidator(ILeaveRequestRepository leaveRequestRepository)
        {
            this._leaveRequestRepository = leaveRequestRepository;

            RuleFor(p => p.Id)
                .NotNull()
                .GreaterThan(0)
                .MustAsync(LeaveRequestMustExist)
                .WithMessage("{PropertyName} does not exist!");
        }

        private async Task<bool> LeaveRequestMustExist(int id, CancellationToken token)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestWithDetails(id);

            return leaveRequest != null;
        }
    }
}
