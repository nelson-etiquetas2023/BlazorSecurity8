using Microsoft.AspNetCore.Identity;
using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Models;

namespace BackendBlazorSecurity8.UnitsOfWork.Interfaces
{
	public interface IUserUnitsOfWork
	{
		Task<User> GetUserAsync(string email);
		Task<IdentityResult> AddUserAsync(User user, string password);
		Task CheckRoleAsync(string roleName);
		Task AddUserToRoleAsync(User user, string roleName);
		Task<bool> IsUserInRoleAsync(User user, string roleName);
		Task<SignInResult> LoginAsync(LoginDTO model);
		Task LogoutAsync();

	}
}
