namespace Revival.Services;

public interface IEmailSender
{
    Task<bool> SendAsync(string subject, string body, CancellationToken cancellationToken = default);
}
