using System.ComponentModel.DataAnnotations;

namespace AgencyCursor.WebApp.Models;

public class InterpreterEmailLog
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public Request Request { get; set; } = null!;
    public int InterpreterId { get; set; }
    public Interpreter Interpreter { get; set; } = null!;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    [Required] public string Status { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}
