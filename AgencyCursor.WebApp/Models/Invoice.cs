using System.ComponentModel.DataAnnotations;

namespace AgencyCursor.WebApp.Models;

public class Invoice
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;
    public decimal Amount { get; set; }
    [Required] public string Status { get; set; } = "Pending";
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? PaidDate { get; set; }
}
