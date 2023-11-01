using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, Unit>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository)
        {
            this._leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            // retrieve domain entity object
            var leaveTypeToDelete = await _leaveTypeRepository.GetByIdAsync(request.Id);
            //var leaveTypeToDelete = _leaveTypeRepository.GetByIdAsync(request.Id) ?? throw new Exception();

            // Verify if record exists
            if (leaveTypeToDelete == null)
            {
                throw new NotFoundException(nameof(LeaveType), request.Id);
            }

            // Delete from Database
            await _leaveTypeRepository.DeleteAsync(leaveTypeToDelete);

            return Unit.Value;
        }
    }
}
