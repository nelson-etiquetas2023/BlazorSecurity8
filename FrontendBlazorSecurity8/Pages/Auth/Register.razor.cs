using FrontendBlazorSecurity8.Repositories;
using FrontendBlazorSecurity8.Services;
using Microsoft.AspNetCore.Components;
using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Enums;

#pragma warning disable IDE0044 // Agregar modificador de solo lectura
#pragma warning disable IDE0051 // Quitar miembros privados no utilizados
#pragma warning disable IDE0052 // Quitar miembros privados no leídos
#pragma warning disable CS0414 // El campo 'Register.loading' está asignado pero su valor nunca se usa
#pragma warning disable IDE0059 // Asignación innecesaria de un valor

namespace FrontendBlazorSecurity8.Pages.Auth
{
	public partial class Register
	{
		private UserDTO userDTO = new();
		private bool loading;
		private string? imageUrl;

		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
		[Inject] private ILoginService LoginService { get; set; } = null!;
		[Inject] private IRepository Repository { get; set; } = null!;

		private void ImageSelected(string imagenBase64) 
		{
			userDTO.Photo = imagenBase64;
			imageUrl = null;
		}

		private async Task CreateUserAsync() 
		{
			userDTO.UserName = userDTO.Email;
			userDTO.UserType = UserType.User;
			loading = true;
			var responseHttp = await Repository.PostAsync<UserDTO, TokenDTO>("/api/accounts/CreateUser", userDTO);
			loading = false;

			if (responseHttp._error) 
			{
				var message = await responseHttp.GetErrorMessageAsync();

				return;
			}

			await LoginService.LoginAsync(responseHttp._response!.Token);
			NavigationManager.NavigateTo("/");
		}
	}
}