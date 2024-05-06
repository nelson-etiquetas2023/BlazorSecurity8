using SharedBlazorSecurity.Models;

namespace BackendBlazorSecurity8.Repositories.Interfaces
{
	public interface ICitiesRepository
	{
		Task<IEnumerable<City>> GetComboAsync(int stateId);
	}
}
