namespace FrontendBlazorSecurity8.Services
{
	public interface ILoginService
	{
		Task LoginAsync(string token);
		Task LogoutAsync();
	}
}
