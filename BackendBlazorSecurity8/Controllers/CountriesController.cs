using Azure;
using BackendBlazorSecurity8.Repositories.Interfaces;
using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedBlazorSecurity.Models;

#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.Controllers
{
	[ApiController]
	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("api/[controller]")]

	public class CountriesController : GenericController<Country>
	{
		private readonly ICountriesUnitOfWork _countriesUnitOfWork;

		public CountriesController(IGenericRepository<Country> unitOfWork, ICountriesUnitOfWork countriesUnitOfWork) : base(unitOfWork)

		{
			_countriesUnitOfWork = countriesUnitOfWork;
		}

		[AllowAnonymous]
		[HttpGet("combo")]
		public async Task<IActionResult> GetComboAsync() 
		{
			return Ok(await _countriesUnitOfWork.GetComboAsync());
		}

		[HttpGet]
		public override async Task<IActionResult> GetAsync() 
		{
			var response = await _countriesUnitOfWork.GetAsync();
			if (response.WasSuccess) 
			{
				return Ok(response.Message);
			}
			return NotFound();
		}

		[HttpGet("{id}")]
		public override async Task<IActionResult> GetAsync(int id) 
		{
			var response = await _countriesUnitOfWork.GetAsync(id);
			if (response.WasSuccess) 
			{
				return Ok(response.Result);
			}
			return NotFound(response.Message);
		}






	}
}
