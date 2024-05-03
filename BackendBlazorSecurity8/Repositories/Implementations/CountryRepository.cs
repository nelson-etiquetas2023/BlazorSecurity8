using BackendBlazorSecurity8.Data;
using BackendBlazorSecurity8.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Models;
using SharedBlazorSecurity.Responses;


#pragma warning disable CS8620 // El argumento no se puede usar para el parámetro debido a las diferencias en la nulabilidad de los tipos de referencia.
#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.Repositories.Implementations
{
	public class CountryRepository : GenericRepository<Country>, ICountriesReposity
	{
		private readonly ApplicationDbContext _context;

		public CountryRepository(ApplicationDbContext context) : base(context)

		{
			_context = context;
		}

		public override async Task<ActionResponse<Country>> GetAsync(int id)
		{

			var country = await _context.Paises
				.Include(c => c.states)
				.ThenInclude(s => s.Cities)
				.FirstOrDefaultAsync(x => x.id == id);

			if (country == null) 
			{
				return new ActionResponse<Country> 
				{
					WasSuccess = false,
					Message = "Pais no existe"
				};
			}

			return new ActionResponse<Country>
			{
				WasSuccess = true,
				Result = country
			};
		}

		public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync()
		{
			var countries = await _context.Paises
				.Include(c => c.states)
				.ToListAsync();

			return new ActionResponse<IEnumerable<Country>>
			{
				WasSuccess = true,
				Result = countries
			};
		}

		public async Task<IEnumerable<Country>> GetComboAsync()
		{

			return await _context.Paises
				.OrderBy(x => x.name)
				.ToListAsync();	
			
		}
	}
}
