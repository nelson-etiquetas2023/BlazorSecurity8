using CurrieTechnologies.Razor.SweetAlert2;
using FrontendBlazorSecurity8.Repositories;
using Microsoft.AspNetCore.Components;
using SharedBlazorSecurity.DTOs;

namespace FrontendBlazorSecurity8.Pages.Auth
{
	public partial class RecoverPassword
	{
		private readonly EmailDTO emailDTO = new();
		private bool loading;

		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
		[Inject] private SweetAlertService Swal { get; set; } = null!;
		[Inject] private IRepository Repository { get; set; } = null!;

		private async Task SendRecoverPasswordEmailTokenAsync() 
		{
			loading = true;
			var responseHttp = await Repository.PostAsync("/api/account/RecoverPassword", emailDTO);
			if (responseHttp.Error) 
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, SweetAlertIcon.Error);
				loading = false;
				return;
			}
			loading = false;
			await Swal.FireAsync("Confirmación", "Se te ha enviado correo electronico con las instrucciones para recuperar tu contraseña.",SweetAlertIcon.Info);

			NavigationManager.NavigateTo("/");

		}
	}
}