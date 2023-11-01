using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.IntegrationTests
{
    public class HrDatabaseContextTests
    {
        private HrDatabaseContext _hrDatabaseContext;
        private IUserService _userService;

        public HrDatabaseContextTests(IUserService userService)
        {
            _userService = userService;
            var dbOptions = new DbContextOptionsBuilder<HrDatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _hrDatabaseContext = new HrDatabaseContext(dbOptions, _userService);
        }

        [Fact]
        public async Task Save_SetDateCreatedValue()
        {
            // Arrange
            var leaveType = new LeaveType { Id = 1, DefaultDays = 10, Name = "Test Vacation" };

            //Act 
            await _hrDatabaseContext.LeaveTypes.AddAsync(leaveType);
            await _hrDatabaseContext.SaveChangesAsync();

            //Assert
            leaveType.DateCreated.ShouldNotBeNull();
        }

        [Fact]
        public async Task Save_SetDateModifiedValue()
        {
            // Arrange
            var leaveType = new LeaveType { Id = 1, DefaultDays = 10, Name = "Test Vacation" };

            //Act 
            await _hrDatabaseContext.LeaveTypes.AddAsync(leaveType);
            await _hrDatabaseContext.SaveChangesAsync();

            //Assert
            leaveType.DateModified.ShouldNotBeNull();
        }
    }
}
