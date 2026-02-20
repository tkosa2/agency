using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Services;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpSettings _settings;
    private readonly AgencyDbContext _db;
    private readonly ILogger<SmtpEmailService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SmtpEmailService(IOptions<SmtpSettings> settings, AgencyDbContext db, ILogger<SmtpEmailService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _settings = settings.Value;
        _db = db;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetBaseUrl()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null) return "http://localhost";
        return $"{request.Scheme}://{request.Host}";
    }

    public async Task BroadcastToInterpretersAsync(Request request, IEnumerable<Interpreter> interpreters)
    {
        var baseUrl = GetBaseUrl();
        foreach (var interpreter in interpreters)
        {
            var log = new InterpreterEmailLog
            {
                RequestId = request.Id,
                InterpreterId = interpreter.Id,
                SentAt = DateTime.UtcNow,
                Status = "Success"
            };
            try
            {
                var responseUrl = $"{baseUrl}/Interpreters/RespondToRequest/{request.Id}/{interpreter.Id}";
                var body = $@"<html><body>
                    <h2>Interpreter Request</h2>
                    <p>Dear {interpreter.FirstName},</p>
                    <p>You have been requested for an interpretation assignment.</p>
                    <p><strong>Date:</strong> {request.AppointmentDate:MM/dd/yyyy}</p>
                    <p><strong>Time:</strong> {request.StartTime} - {request.EndTime}</p>
                    <p><strong>Service Type:</strong> {request.ServiceType}</p>
                    <p><strong>Mode:</strong> {request.Mode}</p>
                    <p><a href='{responseUrl}' style='background:#0d6efd;color:white;padding:10px 20px;text-decoration:none;border-radius:5px;'>Respond Now</a></p>
                    </body></html>";
                await SendEmailAsync(interpreter.Email, $"Interpreter Request #{request.Id}", body);
            }
            catch (Exception ex)
            {
                log.Status = "Failed";
                log.ErrorMessage = ex.Message;
                _logger.LogError(ex, "Failed to send email to interpreter {Id}", interpreter.Id);
            }
            _db.InterpreterEmailLogs.Add(log);
        }
        await _db.SaveChangesAsync();
    }

    public async Task SendAppointmentConfirmationAsync(Appointment appointment)
    {
        var email = appointment.Request?.Requestor?.Email;
        if (string.IsNullOrEmpty(email)) return;
        var body = $@"<html><body>
            <h2>Appointment Confirmed</h2>
            <p>Your appointment on {appointment.AppointmentDate:MM/dd/yyyy} has been confirmed.</p>
            <p>Interpreter: {appointment.Interpreter?.FullName}</p>
            </body></html>";
        await SendEmailAsync(email, $"Appointment Confirmation #{appointment.Id}", body);
    }

    private async Task SendEmailAsync(string to, string subject, string body)
    {
        var toAddress = _settings.IsDevelopment ? _settings.TestEmailAddress : to;
        if (string.IsNullOrEmpty(toAddress)) return;

        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.Username, _settings.Password),
            EnableSsl = true
        };
        using var message = new MailMessage
        {
            From = new MailAddress(_settings.FromEmail, _settings.FromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        message.To.Add(toAddress);
        await client.SendMailAsync(message);
    }
}
