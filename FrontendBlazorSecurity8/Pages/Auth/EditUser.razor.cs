using CurrieTechnologies.Razor.SweetAlert2;
using FrontendBlazorSecurity8.Repositories;
using FrontendBlazorSecurity8.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Models;
using System.Net;
using System.Xml.Serialization;

namespace FrontendBlazorSecurity8.Pages.Auth
{
	[Authorize]
	public partial class EditUser
	{
		private User? user;
		private List<Country>? countries;
		private List<State>? states;
		private List<City>? cities;
		private string? imageUrl;

		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
		[Inject] private SweetAlertService Swal { get; set; } = null!;
		[Inject] IRepository Repository { get; set; } = null!;
		[Inject] ILoginService LoginService { get; set; } = null!;

		protected override async Task OnInitializedAsync() 
		{
			await LoadUserAsync();
			await LoadCountriesAsync();
			await LoadStateAsync(user!.City!.State!.Country!.id);
			await LoadCitiesAsync(user!.City!.State!.Id);

			if (!string.IsNullOrEmpty(user!.Photo)) 
			{
				imageUrl = user.Photo;
				user.Photo = null;
			}
		}
		private async Task LoadUserAsync()
		{
			var httpResponse = await Repository.GetAsync<User>($"/api/account");
			if (httpResponse.Error)
			{
				if (httpResponse.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
				{
					NavigationManager.NavigateTo("/");
					return;
				}
				var messageError = await httpResponse.GetErrorMessageAsync();
				await Swal.FireAsync("Error", messageError, SweetAlertIcon.Error);
				return;
			}
			user = httpResponse.Response;
		}
		private void ImageSelected(string imageBase64)
		{
			user!.Photo = imageBase64;
			imageUrl = null;
		}

		private async Task CountryChangedAsync(ChangeEventArgs e) 
		{
			var selectedCountry = Convert.ToInt32(e.Value);
			states = null;
			cities = null;
			user!.CityId = 0;
			await LoadStateAsync(selectedCountry);
		}

		private async Task StateChangedAsync(ChangeEventArgs e)  
		{
			var selectState = Convert.ToInt32(e.Value!);
			cities = null;
			user!.CityId = 0;
			await LoadCitiesAsync(selectState);
		}
		private async Task LoadCountriesAsync()
		{
			var responseHttp = await Repository.GetAsync<List<Country>>("/api/countries/combo");
			if (responseHttp.Error)
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, SweetAlertIcon.Error);
				return;
			}
			countries = responseHttp.Response;
		}

		private async Task LoadStateAsync(int countryId)
		{
			var httpResponse = await Repository.GetAsync<List<State>>($"api/states/combo/{countryId}");
			if (httpResponse.Error) 
			{
				var message = await httpResponse.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, SweetAlertIcon.Error);
				return;
			}
			states = httpResponse.Response;
		}
		private async Task LoadCitiesAsync(int stateId)
		{
			var responseHttp = await Repository.GetAsync<List<City>>($"/api/cities/combo{stateId}");
			if (responseHttp.Error)
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, SweetAlertIcon.Error);
				return;
			}
			cities = responseHttp.Response;
		}

		private async Task SaveUserAsync() 
		{
			var responseHttp = await Repository.PutAsync<User, TokenDTO>("/api/account", user!);
			if (responseHttp.Error) 
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await Swal.FireAsync("Error", message, SweetAlertIcon.Error);
				return;
			}

			await LoginService.LoginAsync(responseHttp.Response!.Token);
			NavigationManager.NavigateTo("/");
		}
	}
}