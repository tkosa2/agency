using System.ComponentModel.DataAnnotations;

namespace AgencyCursor.WebApp.Models;

public class Interpreter
{
    public int Id { get; set; }
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string Phone { get; set; } = string.Empty;
    [Required] public string Language { get; set; } = string.Empty;
    public string? Certification { get; set; }
    public string? Availability { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<InterpreterResponse> Responses { get; set; } = new List<InterpreterResponse>();
    public string FullName => $"{FirstName} {LastName}";
}
