using BackendBlazorSecurity8.Repositories.Interfaces;
using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Models;

#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.UnitsOfWork.Implementations
{
	public class UserUnitOfWork : IUserUnitsOfWork
	{
		private readonly IUserRepository _userRepository;
        public UserUnitOfWork(IUserRepository userRepository)

        {
			_userRepository = userRepository;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password) => 
			await _userRepository.AddUserAsync(user, password);

		public async Task AddUserToRoleAsync(User user, string roleName) =>
			await _userRepository.AddUserToRoleAsync(user, roleName);

		public async Task CheckRoleAsync(string roleName) =>
			await _userRepository.CheckRoleAsync(roleName);

		public async Task<User> GetUserAsync(string email) =>
			await _userRepository.GetUserAsync(email);

		public async Task<bool> IsUserInRoleAsync(User user, string roleName) =>
			await _userRepository.IsUserInRoleAsync(user, roleName);
	
		public async Task<SignInResult> LoginAsync(LoginDTO model) =>
			await _userRepository.LoginAsync(model);
		
		public async Task LogoutAsync() =>
			await _userRepository.LogoutAsync();
	}
}
