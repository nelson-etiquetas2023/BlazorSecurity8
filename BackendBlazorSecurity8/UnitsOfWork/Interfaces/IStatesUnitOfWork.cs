using SharedBlazorSecurity.Models;
using SharedBlazorSecurity.Responses;

namespace BackendBlazorSecurity8.UnitsOfWork.Interfaces
{
	public interface IStatesUnitOfWork
	{
		Task<ActionResponse<State>> GetAsync(int id);
		Task<ActionResponse<IEnumerable<State>>> GetAsync();
		Task<IEnumerable<State>> GetComboAsync(int countryId);

	}
}
