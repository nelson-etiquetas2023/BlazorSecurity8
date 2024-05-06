using BackendBlazorSecurity8.Data;
using BackendBlazorSecurity8.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedBlazorSecurity.Models;


#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.Repositories.Implementations
{
	public class CitiesRepository : GenericRepository<City>, ICitiesRepository
	{
		private readonly ApplicationDbContext _context;

		public CitiesRepository(ApplicationDbContext context) : base(context)

		{
			_context = context;
		}

		public async Task<IEnumerable<City>> GetComboAsync(int stateId)
		{
			return await _context.Ciudades.
				Where(c => c.StateId  == stateId)
				.OrderBy(c => c.Name)
				.ToListAsync();
		}
	}
}
