using SharedBlazorSecurity.Models;
using SharedBlazorSecurity.Responses;

namespace BackendBlazorSecurity8.Repositories.Interfaces
{
	public interface IStateRepository
	{
		Task<ActionResponse<State>> GetAsync(int id);
		Task<ActionResponse<IEnumerable<State>>> GetAsync();
		Task<IEnumerable<State>> GetComboAsync(int countryId);
	}
}
