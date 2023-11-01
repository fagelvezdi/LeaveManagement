using Azure.Core;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JwtSettings jwtSettings;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.jwtSettings = jwtSettings.Value;
            this.signInManager = signInManager;
        }

        public async Task<AuthResponse> Login(AuthRequest authRequest)
        {
            var user = await userManager.FindByEmailAsync(authRequest.Email);

            if (user == null)
            {
                throw new NotFoundException($"User with email: {authRequest.Email} not found!", authRequest.Email);
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, authRequest.Password, false);

            if (!result.Succeeded) 
            {
                throw new BadRequestException($"Credentials for '{authRequest.Email}' are not valid!");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            var response = new AuthResponse 
            { 
                Email = user.Email,
                Id = user.Id,
                UserName = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            };

            return response;
        }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)

            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest registrationRequest)
        {
            var user = new ApplicationUser
            {
                UserName = registrationRequest.UserName,
                Email = registrationRequest.Email,
                FirstName = registrationRequest.FirstName,
                LastName = registrationRequest.LastName,
                EmailConfirmed = true, //OJO esto se debe validar..!!!!!
            };

            var result = await userManager.CreateAsync(user, registrationRequest.Password);

            if (result.Succeeded) 
            {
                await userManager.AddToRoleAsync(user, "Employee");
                return new RegistrationResponse() { UserId = user.Id };
            }
            else
            {
                StringBuilder sb = new();
                foreach (var err in result.Errors)
                {
                    sb.AppendFormat("*{0}\n", err.Description);
                }
                throw new BadRequestException($"{sb}");
            }
        }
    }
}
