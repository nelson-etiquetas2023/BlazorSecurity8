using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Models;
using SharedBlazorSecurity.Responses;

namespace BackendBlazorSecurity8.UnitsOfWork.Interfaces
{
	public interface ICountriesUnitOfWork
	{
		Task<ActionResponse<Country>> GetAsync(int id);
		Task<ActionResponse<IEnumerable<Country>>> GetAsync();
		Task<IEnumerable<Country>> GetComboAsync();
	}
}
