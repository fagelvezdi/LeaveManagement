namespace HR.LeaveManagement.BlazorUI.Contracts
{
    public interface IAuthenticationService
    {
        /// <summary>
        ///     Handle Login process
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> AuthenticateAsync(
            string email, 
            string password);

        /// <summary>
        ///     Handle user registration
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> RegisterAsync(
            string firstName, 
            string lastName,
            string userName,
            string email, 
            string password);

        Task LogOut();
    }
}
