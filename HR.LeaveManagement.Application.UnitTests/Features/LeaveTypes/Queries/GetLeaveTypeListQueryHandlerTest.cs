using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveTypes.Queries
{
    public class GetLeaveTypeListQueryHandlerTest
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private IMapper _mapper;
        private Mock<IAppLogger<GetLeaveTypeQueryHandler>> _appLogger;

        public GetLeaveTypeListQueryHandlerTest()
        {
            _mockRepo = MockLeaveTypeRepository.GetMockLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveTypeProfile>();
            });

            _mapper = mapperConfig.CreateMapper();

            _appLogger = new Mock<IAppLogger<GetLeaveTypeQueryHandler>>();
        }

        [Fact]
        public async Task GetLeaveTypeListTest()
        {
            var handler = new GetLeaveTypeQueryHandler(_mapper, _mockRepo.Object, _appLogger.Object);

            var result = await handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);
            
            result.ShouldBeOfType<List<LeaveTypeDto>>();
            result.Count.ShouldBe(3);
        }
    }
}
