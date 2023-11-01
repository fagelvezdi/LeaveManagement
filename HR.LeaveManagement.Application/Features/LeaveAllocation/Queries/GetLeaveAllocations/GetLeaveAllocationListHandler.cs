using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations
{
    public class GetLEaveAllocationListHandler : IRequestHandler<GetLeaveAllocationsListQuery, List<LeaveAllocationDto>>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;

        public GetLEaveAllocationListHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
        {
            this._leaveAllocationRepository = leaveAllocationRepository;
            this._mapper = mapper;
        }

        public async Task<List<LeaveAllocationDto>> Handle(GetLeaveAllocationsListQuery request, CancellationToken cancellationToken)
        {
            //To Add later
            // - Get records for specif user
            // - Get allocations per employee

            var leaveAllocations = await _leaveAllocationRepository.GetAllLeaveAllocationsWithDetails();
            var allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);

            return allocations;
        }
    }
}
