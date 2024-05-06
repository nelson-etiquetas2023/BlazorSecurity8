using BackendBlazorSecurity8.Repositories.Interfaces;
using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using SharedBlazorSecurity.Models;

#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.UnitsOfWork.Implementations
{

	public class CitiesUnitOfWork : GenericUnitOfWork<City>, ICitiesUnitOfWork
	{
		
		private readonly ICitiesRepository _citiesRepository;


		public CitiesUnitOfWork(IGenericRepository<City> repository, ICitiesRepository citiesRepository) : 

			base(repository)
		{
			_citiesRepository = citiesRepository;
		}

		public async Task<IEnumerable<City>> GetComboAsync(int stateId) =>
			await _citiesRepository.GetComboAsync(stateId);

	}
}
