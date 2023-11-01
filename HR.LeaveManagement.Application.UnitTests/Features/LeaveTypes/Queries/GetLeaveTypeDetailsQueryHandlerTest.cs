using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveTypes.Queries
{
    public class GetLeaveTypeDetailsQueryHandlerTest
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private IMapper _mapper;

        public GetLeaveTypeDetailsQueryHandlerTest()
        {
            _mockRepo = MockLeaveTypeRepository.GetMockLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveTypeProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        //[Fact]
        //public async Task GetLeaveTypeDetailsTest()
        //{
        //    var handler = new GetLeaveTypeDetailsQueryHandler(_mapper, _mockRepo.Object);
        //    var result = await handler.Handle(new GetLeaveTypeDetailsQuery(), CancellationToken.None);

        //    result.ShouldBeOfType<List<LeaveTypeDto>>();
        //}
    }
}
