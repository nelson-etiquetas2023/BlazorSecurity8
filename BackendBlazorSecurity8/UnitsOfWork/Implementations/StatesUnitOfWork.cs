using BackendBlazorSecurity8.Repositories.Interfaces;
using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using SharedBlazorSecurity.Models;
using SharedBlazorSecurity.Responses;

#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.UnitsOfWork.Implementations
{
	public class StatesUnitOfWork : GenericUnitOfWork<State>, IStatesUnitOfWork
	{
		private readonly IStateRepository _statesRepository;

		public StatesUnitOfWork(IGenericRepository<State> repository, IStateRepository statesRespository) : base(repository)
		{
			_statesRepository = statesRespository;
		}

		public override async Task<ActionResponse<State>> GetAsync(int id) => 
			await _statesRepository.GetAsync(id);

		public override async Task<ActionResponse<IEnumerable<State>>> GetAsync() => 
			await _statesRepository.GetAsync();

		public async Task<IEnumerable<State>> GetComboAsync(int countryId) => 
			await _statesRepository.GetComboAsync(countryId);
	}
}
