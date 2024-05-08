using SharedBlazorSecurity.Responses;

namespace BackendBlazorSecurity8.Helpers
{
	public interface IMailHelper
	{
		ActionResponse<string> SendMail(string toName, string toEmail, string subject, string body);

	}
}
