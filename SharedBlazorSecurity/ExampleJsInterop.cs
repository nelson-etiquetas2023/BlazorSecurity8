using Microsoft.JSInterop;

#pragma warning disable CA1816 // Los métodos Dispose deberían llamar a SuppressFinalize
#pragma warning disable IDE0290 // Usar constructor principal
namespace SharedBlazorSecurity
{
	// This class provides an example of how JavaScript functionality can be wrapped
	// in a .NET class for easy consumption. The associated JavaScript module is
	// loaded on demand when first needed.
	//
	// This class can be registered as scoped DI service and then injected into Blazor
	// components for use.

	public class ExampleJsInterop : IAsyncDisposable
	{
		private readonly Lazy<Task<IJSObjectReference>> moduleTask;

		public ExampleJsInterop(IJSRuntime jsRuntime)

		{
			moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
				"import", "./_content/SharedBlazorSecurity/exampleJsInterop.js").AsTask());
		}

		public async ValueTask<string> Prompt(string message)
		{
			var module = await moduleTask.Value;
			return await module.InvokeAsync<string>("showPrompt", message);
		}

		async ValueTask IAsyncDisposable.DisposeAsync()

		{
			if (moduleTask.IsValueCreated)
			{
				var module = await moduleTask.Value;
				await module.DisposeAsync();
			}
		}
	}
}
