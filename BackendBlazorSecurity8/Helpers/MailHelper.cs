using MailKit.Net.Smtp;
using MimeKit;
using SharedBlazorSecurity.Responses;


namespace BackendBlazorSecurity8.Helpers
{
	public class MailHelper(IConfiguration configuration) : IMailHelper
	{
		private readonly IConfiguration _configuration = configuration;

		public ActionResponse<string> SendMail(string toName, string toEmail, string subject, string body)
		{
			try
			{
				var from = _configuration["Mail:From"];
				var name = _configuration["Mail:Name"];
				var smtp = _configuration["Mail:Smtp"];
				var port = _configuration["Mail:port"];
				var password = _configuration["Mail:Password"];

				var message = new MimeMessage();
				message.From.Add(new MailboxAddress(name, from));
				message.To.Add(new MailboxAddress(toName, toEmail));
				message.Subject = subject;
				 var bodyBuilder = new BodyBuilder
				{
					HtmlBody = body
				};
				message.Body = bodyBuilder.ToMessageBody();

				using (var client = new SmtpClient()) 
				{
					client.Connect(smtp, int.Parse(port!), false);
					client.Authenticate(from, password);
					client.Send(message);
					client.Disconnect(true);
				}
				return new ActionResponse<string> { WasSuccess = true };
			}
			catch (Exception ex)
			{
				return new ActionResponse<string>
				{
					WasSuccess = false,
					Message = ex.Message,
				};
			}
		}
	}
}
