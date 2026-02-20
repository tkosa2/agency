using System.ComponentModel.DataAnnotations;

namespace AgencyCursor.WebApp.Models;

public class Requestor
{
    public int Id { get; set; }
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] public string Phone { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Request> Requests { get; set; } = new List<Request>();
    public string FullName => $"{FirstName} {LastName}";
}
