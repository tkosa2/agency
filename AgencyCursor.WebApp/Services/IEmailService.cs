using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Services;

public interface IEmailService
{
    Task BroadcastToInterpretersAsync(Request request, IEnumerable<Interpreter> interpreters);
    Task SendAppointmentConfirmationAsync(Appointment appointment);
}
