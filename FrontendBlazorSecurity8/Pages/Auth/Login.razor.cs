using CurrieTechnologies.Razor.SweetAlert2;
using FrontendBlazorSecurity8.Repositories;
using FrontendBlazorSecurity8.Services;
using Microsoft.AspNetCore.Components;
using SharedBlazorSecurity.DTOs;


namespace FrontendBlazorSecurity8.Pages.Auth
{
	public partial class Login
	{
		private readonly LoginDTO loginDTO = new();
		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
		[Inject] private SweetAlertService Swal { get; set; } = null!;
		[Inject] private IRepository Repository { get; set; } = null!;
		[Inject] private ILoginService LoginService { get; set; } = null!;

		private async Task LoginAsync() 
		{
			var responseHttp = await Repository.PostAsync<LoginDTO, TokenDTO>("/api/account/login", loginDTO);

			if (responseHttp.Error) 
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, "error");
				return;
			}

			await LoginService.LoginAsync(responseHttp.Response!.Token);
			NavigationManager.NavigateTo("/");
		}
	}
}