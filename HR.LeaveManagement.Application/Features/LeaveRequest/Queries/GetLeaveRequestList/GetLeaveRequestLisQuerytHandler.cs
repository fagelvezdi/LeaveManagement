using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using MediatR.Pipeline;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList
{
    public class GetLeaveRequestLisQuerytHandler : IRequestHandler<GetLeaveRequestListQuery, List<LeaveRequestListDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IUserService _userService;

        public GetLeaveRequestLisQuerytHandler(
            IMapper mapper, 
            ILeaveRequestRepository leaveRequestRepository,
            IUserService userService)
        {
            this._mapper = mapper;
            this._leaveRequestRepository = leaveRequestRepository;
            this._userService = userService;
        }


        public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestListQuery request, CancellationToken cancellationToken)
        {
            var leaveRequests = new List<Domain.LeaveRequest>();
            var requests = new List<LeaveRequestListDto>();

            // Check if it is logged in employee
            if (request.IsLoggedInUser)
            {
                var userId = _userService.UserId;
                leaveRequests = await _leaveRequestRepository.GetAllLeaveRequestWithDetails(userId);

                var employee = await _userService.GetEmployee(userId);
                requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);

                foreach (var req in requests)
                {
                    req.Employee = employee;
                }
            } 
            else
            {
                leaveRequests = await _leaveRequestRepository.GetAllLeaveRequestsWithDetails();
                requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
                foreach (var req in requests)
                {
                    req.Employee = await _userService.GetEmployee(req.RequestingEmployeeId);
                }
            }


            // Fill requests with employee information

            return requests;
        }
    }
}
