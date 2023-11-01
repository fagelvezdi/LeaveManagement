using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest
{
    public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public DeleteLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository)
        {
            this._leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<Unit> Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteLeaveRequestCommandValidator(_leaveRequestRepository);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid leave request", validationResult);
            }

            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

            if (leaveRequest == null)
            {
                throw new NotFoundException(nameof(leaveRequest), request.Id);
            }

            await _leaveRequestRepository.DeleteAsync(leaveRequest);

            return Unit.Value;
        }
    }
}
