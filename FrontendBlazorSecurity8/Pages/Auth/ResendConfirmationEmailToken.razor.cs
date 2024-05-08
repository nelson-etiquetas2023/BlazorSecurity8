using CurrieTechnologies.Razor.SweetAlert2;
using FrontendBlazorSecurity8.Repositories;
using Microsoft.AspNetCore.Components;
using SharedBlazorSecurity.DTOs;

namespace FrontendBlazorSecurity8.Pages.Auth
{
	public partial class ResendConfirmationEmailToken
	{
		private readonly EmailDTO emailDTO = new();

		private bool loading;


		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
		[Inject] private SweetAlertService Swal { get; set; } = null!;
		[Inject] private IRepository Repository { get; set; } = null!;

		private async Task ResendConfirmationEmailTokenAsync()
		{
			loading = true;
			var responseHttp = await Repository.PostAsync("/api/account/ResedToken", emailDTO);
			loading = false;
			if (responseHttp.Error)
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, SweetAlertIcon.Error);
				loading = false;
				return;
			}
			await Swal.FireAsync("Confirmation", "Se te ha enviado un correo electronico con las instrucciones para activar to usuario", SweetAlertIcon.Info);

			NavigationManager.NavigateTo("/");

		}



	}
}