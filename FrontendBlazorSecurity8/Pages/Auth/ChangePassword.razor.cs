using CurrieTechnologies.Razor.SweetAlert2;
using FrontendBlazorSecurity8.Repositories;
using Microsoft.AspNetCore.Components;
using SharedBlazorSecurity.DTOs;

namespace FrontendBlazorSecurity8.Pages.Auth
{
	public partial class ChangePassword
	{
		private ChangePasswordDTO changePasswordDTO = new();
		private bool loading;

		[Inject] private NavigationManager NavigatorManager { get; set; } = null!;
		[Inject] private SweetAlertService Swal { get; set; } = null!;
		[Inject] private IRepository Repository { get; set; } = null!;


		private async Task ChangePasswordAsync() 
		{
			loading = true;
			var responseHttp = await Repository.PostAsync("/api/accounts/changePassword", changePasswordDTO);
			loading = false;
			if (responseHttp.Error) 
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, "error");
				return;
			}
			NavigatorManager.NavigateTo("/editUser");
			var toast = Swal.Mixin(new SweetAlertOptions
			{
				Toast = true,
				Position = SweetAlertPosition.BottomEnd,
				ShowConfirmButton = true,
				Timer = 3000
			});
			await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Contraseña cambiada con exito."); 
		}
	}
}