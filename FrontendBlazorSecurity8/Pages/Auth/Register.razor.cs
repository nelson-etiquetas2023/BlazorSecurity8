using FrontendBlazorSecurity8.Services;
using Microsoft.AspNetCore.Components;
using SharedBlazorSecurity.DTOs;

namespace FrontendBlazorSecurity8.Pages.Auth
{
	public partial class Register
	{
		private UserDTO userDTO = new();
		private bool loading;
		private string? imageUrl;

		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
		[Inject] private ILoginService LoginService { get; set; } = null!;

		private async Task CreateUserAsync() 
		{
			userDTO.UserName = userDTO.Email;       
			loading = true;




			await LoginService.LoginAsync("");
			NavigationManager.NavigateTo("/");
		}


	}
}