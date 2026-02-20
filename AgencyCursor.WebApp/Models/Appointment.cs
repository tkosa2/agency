using System.ComponentModel.DataAnnotations;

namespace AgencyCursor.WebApp.Models;

public class Appointment
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public Request Request { get; set; } = null!;
    public int InterpreterId { get; set; }
    public Interpreter Interpreter { get; set; } = null!;
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? LocationDetails { get; set; }
    [Required] public string Status { get; set; } = "Assigned";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
