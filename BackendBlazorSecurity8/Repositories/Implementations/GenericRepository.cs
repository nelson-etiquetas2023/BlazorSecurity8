using BackendBlazorSecurity8.Data;
using BackendBlazorSecurity8.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedBlazorSecurity.Responses;
using System.Linq.Expressions;

#pragma warning disable CA1822 // Marcar miembros como static
namespace BackendBlazorSecurity8.Repositories.Implementations
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		private readonly DbSet<T> _entity;

		public GenericRepository(ApplicationDbContext context)
        {
			_context = context;
			_entity = _context.Set<T>();
		}

        public virtual async Task<ActionResponse<T>> AddAsync(T entity)
		{
			_context.Add(entity);
			try
			{
				await _context.SaveChangesAsync();
				return new ActionResponse<T>
				{
					WasSuccess = true,
					Result = entity
				};
			}
			catch (DbUpdateException)
			{
				return DbUpdateExceptionActionResponse();

			}
			catch (Exception ex) 
			{
				return ExceptionActionResponse(ex);
			}
		}

		public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
		{
			var row = await _entity.FindAsync(id);
			if (row == null) 
			{
				return new ActionResponse<T>
				{
					WasSuccess = false,
					Message =  "Resgistro no encontrado"
				};
			}
			try
			{
				_entity.Remove(row);
				await _context.SaveChangesAsync();
				return new ActionResponse<T>
				{
					WasSuccess = true
				};
			}
			catch
			{
				return new ActionResponse<T>
				{
					WasSuccess = false,
					Message = "No se puede Borrar, porque tiene registros relacionados"
				};
			}



		}

		public virtual async Task<ActionResponse<T>> GetAsync(int id)
		{
			var row = await _entity.FindAsync(id);
			if (row == null)
			{
				return new ActionResponse<T>
				{
					WasSuccess = false,
					Message = "Resgistro no encontrado"
				};
			}
			return new ActionResponse<T>
			{
				WasSuccess = true,
				Result = row
			};
		}

		public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
		{
			return new ActionResponse<IEnumerable<T>>
			{
				WasSuccess = true,
				Result = await _entity.ToListAsync()
			};
		}

		public virtual async Task<ActionResponse<T>> UpdateAsync(T entity)
		{
			_context.Update(_entity);
			try
			{
				await _context.SaveChangesAsync();
				return new ActionResponse<T>
				{
					WasSuccess = true,
					Result = entity
				};
			}
			catch (DbUpdateException)
			{
				return DbUpdateExceptionActionResponse();

			}
			catch (Exception ex)
			{
				return ExceptionActionResponse(ex);
			}
		}
		

		private ActionResponse<T> DbUpdateExceptionActionResponse()
		{
			return new ActionResponse<T>
			{
				WasSuccess = false,
				Message = "Ya existe el registro que estas intentando crear"
			};
		}
		
		private ActionResponse<T> ExceptionActionResponse(Exception ex)
#pragma warning restore CA1822 // Marcar miembros como static
		{
			return new ActionResponse<T>
			{
				WasSuccess = false,
				Message = ex.Message
			};
		}
	}
}
