using Microsoft.JSInterop;

namespace FrontendBlazorSecurity8.Helpers
{
	public static class IJSRuntimeExtensionMethods
	{
		public static ValueTask<object> SetLocalStorage(this IJSRuntime js, string key, string content) 
		{
			return js.InvokeAsync<object>("LocalStorage.setItem", key, content);
		}

		public static ValueTask<object> GetLocalStorage(this IJSRuntime js, string key ) 
		{
			return js.InvokeAsync<object>("LocalStorage.getItem", key); 
		}

		public static ValueTask<object> RemoveLocalStorage(this IJSRuntime js, string key) 
		{
			return js.InvokeAsync<object>("LocalStorage.removeItem", key);
		}
	}
}
