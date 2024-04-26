using System.Net;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0290 // Usar constructor principal
#pragma warning disable IDE1006 // Estilos de nombres
namespace FrontendBlazorSecurity8.Repositories
{
	public class HttpResponseWrapper<T>
	{

		public T? _response { get; }

		public bool _error { get; }
		public HttpResponseMessage _httpResponseMessage { get; }

		public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)

        {
			_response = response;
			_error  = error;
			_httpResponseMessage = httpResponseMessage;
		}

		public async Task<string?> GetErrorMessageAsync() 
		{
			if (!_error) 
			{
				return null;
			}

			var statusCode = _httpResponseMessage.StatusCode;
			if (statusCode == HttpStatusCode.NotFound) 
			{
				return "Recurso no encontrado.";
			}
			if (statusCode == HttpStatusCode.BadRequest) 
			{
				return await _httpResponseMessage.Content.ReadAsStringAsync();
			}
			if (statusCode == HttpStatusCode.Unauthorized) 
			{
				return "Tienes que estar logueado para ejecutar esta operación.";
			}
			if (statusCode == HttpStatusCode.Forbidden)
			{
				return "No tienes permisos para hacer esta operación.";
			}

			return "Ha ocurrido un error inesperado.";

		}

	}
}
