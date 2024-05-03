using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Models;
using SharedBlazorSecurity.Responses;

namespace BackendBlazorSecurity8.Repositories.Interfaces
{
	public interface ICountriesReposity
	{
		Task<ActionResponse<Country>> GetAsync(int id);
		Task<ActionResponse<IEnumerable<Country>>> GetAsync();
		Task<IEnumerable<Country>> GetComboAsync();
	}
}
