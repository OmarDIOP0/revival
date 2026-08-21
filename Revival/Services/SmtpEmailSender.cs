using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Revival.Configuration;

namespace Revival.Services;

/// <summary>
/// Sends the contact form to the clinic's inbox. When no SMTP host is configured yet
/// (fresh install), it logs the message instead of failing the request.
/// </summary>
public class SmtpEmailSender(
    IOptions<SmtpSettings> smtpOptions,
    IOptions<SiteSettings> siteOptions,
    ILogger<SmtpEmailSender> logger) : IEmailSender
{
    private readonly SmtpSettings _smtp = smtpOptions.Value;
    private readonly SiteSettings _site = siteOptions.Value;

    public async Task<bool> SendAsync(string subject, string body, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_smtp.Host) || string.IsNullOrWhiteSpace(_site.Email))
        {
            logger.LogWarning("SMTP is not configured yet — contact message was not emailed. Subject: {Subject}\n{Body}", subject, body);
            return false;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(string.IsNullOrWhiteSpace(_smtp.From) ? _site.Email : _smtp.From),
            Subject = subject,
            Body = body,
        };
        message.To.Add(_site.Email);

        using var client = new SmtpClient(_smtp.Host, _smtp.Port)
        {
            EnableSsl = _smtp.EnableSsl,
            Credentials = string.IsNullOrWhiteSpace(_smtp.User)
                ? null
                : new NetworkCredential(_smtp.User, _smtp.Password),
        };

        await client.SendMailAsync(message, cancellationToken);
        return true;
    }
}
