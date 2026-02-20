using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AgencyCursor.WebApp.Models;

public class Request
{
    public int Id { get; set; }
    public int RequestorId { get; set; }
    [ValidateNever] public Requestor Requestor { get; set; } = null!;
    public string? RequestName { get; set; }
    public int NumberOfDeafIndividuals { get; set; }
    [Required] public string IndividualType { get; set; } = string.Empty;
    [Required] public string ServiceType { get; set; } = string.Empty;
    public string? ServiceTypeOther { get; set; }
    [Required] public string Mode { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? Address { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? VirtualMeetingLink { get; set; }
    public string? GenderPreference { get; set; }
    public string? PreferredInterpreter { get; set; }
    public string? SpecialRequirements { get; set; }
    public string? AdditionalInfo { get; set; }
    [Required] public string Status { get; set; } = "New Request";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<InterpreterResponse> Responses { get; set; } = new List<InterpreterResponse>();
    public ICollection<InterpreterEmailLog> EmailLogs { get; set; } = new List<InterpreterEmailLog>();
}
