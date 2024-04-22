 using Microsoft.AspNetCore.Identity;
using SharedBlazorSecurity.Models;

namespace BackendBlazorSecurity8.Repositories.Interfaces
{
	public interface IUserRepository
	{
		Task<User> GetUserAsync(string email);
		Task<IdentityResult> AddUserAsync(User user, string password);
		Task CheckRoleAsync(string roleName);
		Task AddUserToRoleAsync(User user, string roleName);
		Task<bool> IsUserInRoleAsync(User user, string roleName);
	}
}
