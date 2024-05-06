using BackendBlazorSecurity8.Repositories.Interfaces;
using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedBlazorSecurity.Models;

namespace BackendBlazorSecurity8.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	//[Authorize("AuthenticationSchemes = JwtBearerDefault.AuthenticationSchema")]

	public class CitiesController(IGenericRepository<City> unitOfWork, ICitiesUnitOfWork citiesUnitOfWork) : GenericController<City>(unitOfWork)
	{
		[AllowAnonymous]
		[HttpGet("combo/{stateId:int}")]
		public async Task<IActionResult> GetComboAsync(int stateId) 
		{
			return Ok(await citiesUnitOfWork.GetComboAsync(stateId));
		}
	}
}
