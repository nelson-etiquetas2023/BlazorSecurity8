using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SharedBlazorSecurity.Models;

namespace BackendBlazorSecurity8.UnitsOfWork.Interfaces
{
	public interface ICitiesUnitOfWork
	{
		Task<IEnumerable<City>> GetComboAsync(int stateId);
	}
}
