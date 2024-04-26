using FrontendBlazorSecurity8.Services;
using Microsoft.AspNetCore.Components;

namespace FrontendBlazorSecurity8.Pages.Auth
{
	public partial class Logout
	{
		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
		[Inject] private ILoginService LoginService { get; set; } = null!;

		protected override async Task OnInitializedAsync()
		{
			await LoginService.LogoutAsync();
			NavigationManager.NavigateTo("/");
		}
	}
}