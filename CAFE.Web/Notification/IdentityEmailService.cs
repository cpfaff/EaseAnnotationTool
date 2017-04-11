using System;
using System.Threading.Tasks;
using CAFE.Core.Configuration;
using CAFE.Core.Notification;
using Microsoft.AspNet.Identity;
using MailKit.Net.Smtp;
using MimeKit;

namespace CAFE.Web.Notification
{
	public class IdentityEmailService : IEmailService, IIdentityMessageService
	{
		private IConfigurationProvider _configurationProvider;
		private IEmailTemplatesRenderer _emailTemplatesRenderer;

		public IdentityEmailService(IConfigurationProvider configurationProvider, IEmailTemplatesRenderer emailTemplatesRenderer)
		{
			_configurationProvider = configurationProvider;
			_emailTemplatesRenderer = emailTemplatesRenderer;
		}

		public async Task SendAsync(IdentityMessage message)
		{
			try
			{
				var emailParameters = _configurationProvider.Get<EmailConfiguration>();

				var mess = new MimeMessage();
				mess.From.Add(new MailboxAddress(emailParameters.NameFrom, emailParameters.Login));
				mess.To.Add(new MailboxAddress("", message.Destination));
				mess.Subject = message.Subject;

				mess.Body = new TextPart("html")
				{
					Text = message.Body
				};

				using (var client = new SmtpClient())
				{
					// For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
					client.ServerCertificateValidationCallback = (s, c, h, e) => emailParameters.IsSslRequired;

					client.Connect(emailParameters.SmtpHost, emailParameters.SmtpPort, emailParameters.IsSslRequired);

					// Note: since we don't have an OAuth2 token, disable
					// the XOAUTH2 authentication mechanism.
					client.AuthenticationMechanisms.Remove("XOAUTH2");

					// Note: only needed if the SMTP server requires authentication
					client.Authenticate(emailParameters.Login, emailParameters.Password);

					client.Send(mess);
					client.Disconnect(true);
				}

				await Task.Delay(0);
			}
			catch (Exception exception)
			{
				//TODO: log here
				throw exception;
			}

		}
	}
}
