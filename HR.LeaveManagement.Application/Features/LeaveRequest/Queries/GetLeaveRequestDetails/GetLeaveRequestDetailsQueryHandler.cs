using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails
{
    public class GetLeaveRequestDetailsQueryHandler : IRequestHandler<GetLeaveRequestDetailsQuery, LeaveRequestDetailsDto>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IUserService _userService;

        public GetLeaveRequestDetailsQueryHandler(
            IMapper mapper,
            ILeaveRequestRepository leaveRequestRepository,
            IUserService userService)
        {
            this._mapper = mapper;
            this._leaveRequestRepository = leaveRequestRepository;
            this._userService = userService;
        }

        public async Task<LeaveRequestDetailsDto> Handle(GetLeaveRequestDetailsQuery request, CancellationToken cancellationToken)
        {
            var leaveRequest = _mapper.Map<LeaveRequestDetailsDto>(await _leaveRequestRepository.GetLeaveRequestWithDetails(request.Id));

            if (leaveRequest == null)
            {
                throw new NotFoundException(nameof(LeaveRequest), request.Id);
            }
            // Add Employee details as needed
            leaveRequest.Employee = await _userService.GetEmployee(leaveRequest.RequestingEmployeeId);

            return leaveRequest;
        }
    }
}
