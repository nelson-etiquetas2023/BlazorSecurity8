using CurrieTechnologies.Razor.SweetAlert2;
using FrontendBlazorSecurity8.Repositories;
using FrontendBlazorSecurity8.Services;
using Microsoft.AspNetCore.Components;
using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Enums;
using SharedBlazorSecurity.Models;

#pragma warning disable IDE0044 // Agregar modificador de solo lectura
#pragma warning disable CS0414 // El campo 'Register.loading' está asignado pero su valor nunca se usa
namespace FrontendBlazorSecurity8.Pages.Auth
{
	public partial class Register
	{
		private UserDTO userDTO = new();
		private List<Country>? countries;
		private List<State>? states;
		private List<City>? cities;
		private bool loading;
		private string? imageUrl;

		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
		[Inject] private ILoginService LoginService { get; set; } = null!;
		[Inject] private SweetAlertService Swal { get; set; } = null!;
		[Inject] private IRepository Repository { get; set; } = null!;


		protected override async Task OnInitializedAsync()
		{
			await LoadCountriesAsync();
		}

		private async Task LoadCountriesAsync()
		{
			var responseHttp = await Repository.GetAsync<List<Country>>($"/api/Countries/combo");


			if (responseHttp.Error) 
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, "error");
				return;
			}

			countries = responseHttp.Response;
		}

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
			var responseHttp = await Repository.PostAsync<UserDTO, TokenDTO>("/Api/Account/CreateUser", userDTO);
			loading = false;

			if (responseHttp.Error) 
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, "error");
				return;
			}

			await LoginService.LoginAsync(responseHttp.Response!.Token);
			NavigationManager.NavigateTo("/");
		}

		private async Task CountryChangedAsync(ChangeEventArgs e) 
		{
			var selectedCountry = Convert.ToInt32(e.Value!);
			states = null;
			cities = null;
			userDTO.CityId = 0;
			await LoadStatesAsync(selectedCountry);
		}

		private async Task LoadStatesAsync(int countryId)
		{
			var responseHttp = await Repository.GetAsync<List<State>>($"/api/states/combo/{countryId}");

			if (responseHttp.Error) 
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, "error");
				return;
			}
			states = responseHttp.Response;
		}

		private async Task StateChangedAsync(ChangeEventArgs e) 
		{
			var selectedState = Convert.ToInt32(e.Value!);
			cities = null;
			userDTO.CityId = 0;
			await LoadCitiesAsync(selectedState);

		}

		private async Task LoadCitiesAsync(int stateId)
		{
			var responseHttp = await Repository.GetAsync<List<City>>($"/api/cities/combo/{stateId}");

			if (responseHttp.Error) 
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, "error");
				return;
			}
			cities = responseHttp.Response;
		}
	}
}