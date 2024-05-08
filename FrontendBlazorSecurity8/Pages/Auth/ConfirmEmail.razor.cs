using CurrieTechnologies.Razor.SweetAlert2;
using FrontendBlazorSecurity8.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FrontendBlazorSecurity8.Pages.Auth
{
	public partial class ConfirmEmail
	{
		private string? message;

		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
		[Inject] private SweetAlertService Swal { get; set; } = null!;
		[Inject] private IRepository Repository { get; set; } = null!;

		[Parameter, SupplyParameterFromQuery] public string UserId { get; set; } = string.Empty;
		[Parameter, SupplyParameterFromQuery] public string Token { get; set; } = string.Empty;

		protected async Task ConfirmAccountAsync() 
		{
			var responseHttp = await Repository.GetAsync($"/api/account/ConfirmEmail/?userId={UserId}&token={Token}");
			if (responseHttp.Error)
			{
				message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, "error");
				NavigationManager.NavigateTo("/");
				return;
			}

			await Swal.FireAsync("Confirmación", "Gracias por confirmar su email, ahora puedes ingresar al sistema", SweetAlertIcon.Info);

			NavigationManager.NavigateTo("/Login");
		
		}
	}
}