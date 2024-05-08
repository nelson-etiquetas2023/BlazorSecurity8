using CurrieTechnologies.Razor.SweetAlert2;
using FrontendBlazorSecurity8.Repositories;
using Microsoft.AspNetCore.Components;
using SharedBlazorSecurity.DTOs;

namespace FrontendBlazorSecurity8.Pages.Auth
{
	public partial class ResetPassword
	{
		private ResetPasswordDTO resetPasswordDTO = new();
		private bool loading;

		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
		[Inject] private SweetAlertService Swal { get; set; } = null!;
		[Inject] private IRepository Repository { get; set; } = null!;
		[Parameter, SupplyParameterFromQuery] public string Token { get; set; } = null!;

		private async Task ChangePasswordAsync() 
		{
			resetPasswordDTO.Token = Token;
			loading = true;
			var responseHttp = await Repository.PostAsync("/api/account/ResetPassword", resetPasswordDTO);
			loading = false;
			if (responseHttp.Error) 
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, SweetAlertIcon.Error);
				loading = false;
				return;
			}

			await Swal.FireAsync("Confirmación", "Contraseña cambiada con exito, ahora puede ingresar con su nueva contraseña", SweetAlertIcon.Info);
			NavigationManager.NavigateTo("/Login");
		}
	}
}