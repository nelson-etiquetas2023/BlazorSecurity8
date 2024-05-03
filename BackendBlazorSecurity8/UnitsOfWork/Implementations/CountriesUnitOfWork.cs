using BackendBlazorSecurity8.Repositories.Interfaces;
using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using SharedBlazorSecurity.DTOs;
using SharedBlazorSecurity.Models;
using SharedBlazorSecurity.Responses;

#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.UnitsOfWork.Implementations
{
	public class CountriesUnitOfWork : GenericUnitOfWork<Country>, ICountriesUnitOfWork
	{
		private readonly ICountriesReposity _countriesRepository;


		public CountriesUnitOfWork(IGenericRepository<Country> repository, ICountriesReposity countriesRepository) : base(repository)

		{
			_countriesRepository = countriesRepository;
		}

		public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync() => 
			await _countriesRepository.GetAsync();
		
		public override async Task<ActionResponse<Country>> GetAsync(int id) => 
			await _countriesRepository.GetAsync(id);
		
		public async Task<IEnumerable<Country>> GetComboAsync() => 
			await _countriesRepository.GetComboAsync();
		
	}
}
