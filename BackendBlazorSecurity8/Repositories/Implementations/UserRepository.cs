using BackendBlazorSecurity8.Data;
using BackendBlazorSecurity8.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Models;
using System.Reflection.Metadata.Ecma335;


#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.Repositories.Implementations
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _Context;
		private readonly UserManager<User> _UserManager;
		private readonly RoleManager<IdentityRole> _RoleManager;
		private readonly SignInManager<User> _SignInManager;

		public UserRepository(ApplicationDbContext context, UserManager<User> usemanager, RoleManager<IdentityRole> roleManager, SignInManager<User> SignManager )  

        {
			_Context = context;
			_UserManager = usemanager;
			_RoleManager = roleManager;	
			_SignInManager = SignManager;
		}

		public async Task<string> GeneratePasswordResetTokenAsync(User user)
		{
			return await _UserManager.GeneratePasswordResetTokenAsync(user);
		}

		public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
		{
			return await _UserManager.ResetPasswordAsync(user, token, password);
		}

		public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
		{
			return await _UserManager.GenerateEmailConfirmationTokenAsync(user);
		}

		public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
		{
			return await _UserManager.ConfirmEmailAsync(user, token);
		}

		public async Task<IdentityResult> AddUserAsync(User user, string password)
		{
			return await _UserManager.CreateAsync(user, password);
		}

		public async Task AddUserToRoleAsync(User user, string roleName)
		{
			await _UserManager.AddToRoleAsync(user, roleName);	
		}

		public Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
		{
			return _UserManager.ChangePasswordAsync(user, currentPassword, newPassword);
		}

		public async Task CheckRoleAsync(string roleName)
		{
			var roleExist = await _RoleManager.RoleExistsAsync(roleName);
			if (!roleExist) 
			{
				await _RoleManager.CreateAsync(new IdentityRole
				{
					Name = roleName
				});
			}
		}

		public async Task<User> GetUserAsync(string email)
		{
			var user = await _Context.Users
				.Include(u => u.City!)
				.ThenInclude(u => u.State!)
				.ThenInclude(u => u.Country!)
				.FirstOrDefaultAsync(x => x.Email == email);

			return user!;
		}

		public async Task<User> GetUserAsync(Guid userId) 
		{

			var user = await _Context.Users
				.Include(u => u.City!)
				.ThenInclude(c => c.State!)
				.ThenInclude(s => s.CountryId)
				.FirstOrDefaultAsync(x => x.Id == userId.ToString());

			return user!;

		}

		public async Task<bool> IsUserInRoleAsync(User user, string roleName)
		{
			return await _UserManager.IsInRoleAsync(user, roleName);
		}

		public async Task<SignInResult> LoginAsync(LoginDTO model)
		{
			return await _SignInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
		}

		public async Task LogoutAsync()
		{
			await _SignInManager.SignOutAsync();
		}

		public async Task<IdentityResult> UpdateUserAsync(User user) 
		{
			return await _UserManager.UpdateAsync(user);
		}

		
	}
}
