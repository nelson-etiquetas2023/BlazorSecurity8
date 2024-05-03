using BackendBlazorSecurity8.Repositories.Interfaces;
using BackendBlazorSecurity8.UnitsOfWork.Implementations;
using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedBlazorSecurity.Models;
using SharedBlazorSecurity.Responses;

#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.Controllers
{
	[ApiController]
	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("api/[controller]")]
	public class StatesController : GenericController<State>
	{
		private readonly IStatesUnitOfWork _statesUnitOfWork;	


		public StatesController(IGenericRepository<State> unitOfWork, IStatesUnitOfWork stateUnitOfWork) : base(unitOfWork)
		{
			_statesUnitOfWork = stateUnitOfWork;
		}

		[AllowAnonymous]
		[HttpGet("combo/{countryId:int}")]
		public async Task<IActionResult> GetComboAsync(int countryId) 
		{
			return Ok(await _statesUnitOfWork.GetComboAsync(countryId));
		}

		[HttpGet("full")]
		public override async Task<IActionResult> GetAsync() 
		{
			var response = await _statesUnitOfWork.GetAsync();
			if (response.WasSuccess) 
			{
				return Ok(response.Result);
			}
			return BadRequest();
		}
		[HttpGet("{id}")]
		public override async Task<IActionResult> GetAsync(int id) 
		{
			var response = await _statesUnitOfWork.GetAsync(id);
			if (response.WasSuccess) 
			{
				return Ok(response.Result);
			}
			return NotFound(response.Message);
		}
	}
}
