using FrontendBlazorSecurity8.Helpers;
using FrontendBlazorSecurity8.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

#pragma warning disable CA1822 // Marcar miembros como static
#pragma warning disable IDE0290 // Usar constructor principal
namespace FrontendBlazorSecurity8.AuthenticationProviders
{
	public class AuthenticationProviderJWT : AuthenticationStateProvider, ILoginService
	{
		private readonly IJSRuntime _jsRuntime;
		private readonly HttpClient _httpClient;
		private readonly string _tokenkey;
		private readonly AuthenticationState _anonimous;

        public AuthenticationProviderJWT(IJSRuntime jsRuntime, HttpClient httpClient)
        {
			_jsRuntime = jsRuntime;
			_httpClient = httpClient;
			_tokenkey = "TOKEN_KEY";
			_anonimous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
		}

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var token = await _jsRuntime.GetLocalStorage(_tokenkey);
			if (token is null)
			{
				return _anonimous;
			}

			return BuildAutheticationState(token.ToString()!);
		}

		private AuthenticationState BuildAutheticationState(string token)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
			var claims = ParseClaimsFromJWT(token);
			return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
		}

		private IEnumerable<Claim> ParseClaimsFromJWT(string token) 
		{
			var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			var unserializedToken = jwtSecurityTokenHandler.ReadJwtToken(token);
			return unserializedToken.Claims;
		}

		public async Task LoginAsync(string token)
		{
			await _jsRuntime.SetLocalStorage(_tokenkey, token);
			var authState = BuildAutheticationState(token);
			NotifyAuthenticationStateChanged(Task.FromResult(authState));
		}

		public async Task LogoutAsync()
		{
			await _jsRuntime.RemoveLocalStorage(_tokenkey);
			_httpClient.DefaultRequestHeaders.Authorization = null;
			NotifyAuthenticationStateChanged(Task.FromResult(_anonimous));
		}
	}
}
