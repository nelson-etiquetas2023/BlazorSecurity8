 using Microsoft.AspNetCore.Identity;
using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Models;

namespace BackendBlazorSecurity8.Repositories.Interfaces
{
	public interface IUserRepository
	{
		Task<string> GeneratePasswordResetTokenAsync(User user);
		Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
		Task<string> GenerateEmailConfirmationTokenAsync(User user);
		Task<IdentityResult> ConfirmEmailAsync(User user, string token);
		Task<User> GetUserAsync(string email);
		Task<IdentityResult> AddUserAsync(User user, string password);
		Task CheckRoleAsync(string roleName);
		Task AddUserToRoleAsync(User user, string roleName);
		Task<bool> IsUserInRoleAsync(User user, string roleName);
		Task<SignInResult> LoginAsync(LoginDTO model);
		Task LogoutAsync();

	}
}
