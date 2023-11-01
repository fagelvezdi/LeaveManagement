using Microsoft.AspNetCore.Components;
using HR.LeaveManagement.BlazorUI.Models;
using HR.LeaveManagement.BlazorUI.Contracts;

namespace HR.LeaveManagement.BlazorUI.Pages
{
    public partial class Login
    {
        public LoginVM Model { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string Message { get; set; }

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; }

        public Login()
        {

        }

        protected override void OnInitialized()
        {
            Model = new LoginVM();
        }

        protected async Task HandleLogin()
        {
            var isAuthenticated = await AuthenticationService.AuthenticateAsync(Model.Email, Model.Password);

            if (isAuthenticated)
            {
                NavigationManager.NavigateTo("home");
            }

            Message = "Username/Password combination unknown!";
        }
    }
}