using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HR.LeaveManagement.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(
            UserManager<ApplicationUser> userManager, 
            IHttpContextAccessor contextAccessor)
        {
            this._userManager = userManager;
            this._contextAccessor = contextAccessor;
        }

        public string UserId { get => _contextAccessor.HttpContext?.User?.FindFirstValue("uid"); }

        public async Task<Employee> GetEmployee(string userId)
        {
            var employee = await _userManager.FindByIdAsync(userId);
            return new Employee
            {
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Id = employee.Id
            };
        }

        public async Task<List<Employee>> GetEmployees()
        {
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            return employees.Select(q => new Employee
            {
                Id = q.Id,
                FirstName = q.FirstName,
                LastName = q.LastName,
                Email = q.Email,
            }).ToList();
        }
    }
}
