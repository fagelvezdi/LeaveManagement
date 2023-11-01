using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation
{
    public class DeleteLeaveAllocationCommandValidator : AbstractValidator<DeleteLeaveAllocationCommand>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;

        public DeleteLeaveAllocationCommandValidator(ILeaveAllocationRepository leaveAllocationRepository)
        {
            this._leaveAllocationRepository = leaveAllocationRepository;

            RuleFor(p => p.Id)
                .GreaterThan(0)
                .MustAsync(LeaveAllocationMustExist)
                .WithMessage("{PropertyName} does not exist!");
        }

        private async Task<bool> LeaveAllocationMustExist(int id, CancellationToken token)
        {
            var leaveAllocation = await _leaveAllocationRepository.GetByIdAsync(id);
            return leaveAllocation != null;
        }
    }
}
