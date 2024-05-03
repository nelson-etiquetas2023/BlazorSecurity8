using BackendBlazorSecurity8.Data;
using BackendBlazorSecurity8.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedBlazorSecurity.Models;
using SharedBlazorSecurity.Responses;
using System.Reflection.Metadata.Ecma335;

#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.Repositories.Implementations
{
	public class StateRepository : GenericRepository<State>, IStateRepository
	{
		private readonly ApplicationDbContext _context;

		public StateRepository(ApplicationDbContext context) : base(context)

		{
			_context = context;
		}

		public override async Task<ActionResponse<State>> GetAsync(int id)
		{
			var state = await _context.Estados
				.Include(s => s.Cities)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (state == null) 
			{
				return new ActionResponse<State>
				{
					WasSuccess = false,
					Message = "Estado no existe"
				};
			}

			return new ActionResponse<State>
			{
				WasSuccess = true,
				Result = state
			};
		}

		public override async Task<ActionResponse<IEnumerable<State>>> GetAsync()
		{
			var states = await _context.Estados
				.Include(x => x.Cities).ToListAsync();
				

			return new ActionResponse<IEnumerable<State>>
			{
				WasSuccess = true,
				Result = states
			};
		}

		public async Task<IEnumerable<State>> GetComboAsync(int countryId)
		{
			return await _context.Estados
				.Where(s => s.CountryId == countryId)
				.OrderBy(s => s.Name)
				.ToListAsync();
		}
	}
}
