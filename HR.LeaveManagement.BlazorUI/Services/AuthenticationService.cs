using Blazored.LocalStorage;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Providers;
using HR.LeaveManagement.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace HR.LeaveManagement.BlazorUI.Services
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationService(
            IClient client, 
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider) : base(client, localStorage)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            try
            {
                AuthRequest authenticationRequest = new()
                {
                    Email = email,
                    Password = password
                };

                var authenticationResponse = await _client.LoginAsync(authenticationRequest);
                if (!string.IsNullOrEmpty(authenticationResponse.Token))
                {
                    await _localStorage.SetItemAsync("token", authenticationResponse.Token);

                    // Set claims in Blazor and login state
                    await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggenIn();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public async Task LogOut()
        {
            //Remove claims in blazor and validate login state
            await ((ApiAuthenticationStateProvider) _authenticationStateProvider).LoggedOut();
        }

        public async Task<bool> RegisterAsync(string firstName, string lastName, string userName, string email, string password)
        {
            try
            {
                RegistrationRequest registrationRequest = new()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = userName,
                    Email = email,
                    Password = password
                };

                var response = await _client.RegisterAsync(registrationRequest);

                if (!string.IsNullOrEmpty(response.UserId))
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
