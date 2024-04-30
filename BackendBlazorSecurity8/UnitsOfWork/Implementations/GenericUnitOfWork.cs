﻿using BackendBlazorSecurity8.UnitsOfWork.Interfaces;
using SharedBlazorSecurity.Responses;

#pragma warning disable IDE0290 // Usar constructor principal
namespace BackendBlazorSecurity8.UnitsOfWork.Implementations
{
	public class GenericUnitOfWork<T> : IGenericUnitOfWork<T> where T : class
	{
		private readonly IGenericUnitOfWork<T> _repository;

		public GenericUnitOfWork(IGenericUnitOfWork<T> repository)
        {
			_repository = repository;
		}

		public virtual async Task<ActionResponse<T>> AddAsync(T model) => await _repository.AddAsync(model);

		public virtual async Task<ActionResponse<T>> DeleteAsync(int id) => await _repository.DeleteAsync(id);

		public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync() => await _repository.GetAsync();

		public virtual async Task<ActionResponse<T>> GetAsync(int id) => await _repository.GetAsync(id);

		public virtual async Task<ActionResponse<T>> UpdateAsync(T model) => await _repository.UpdateAsync(model);		
	}
}
