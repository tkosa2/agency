using System.ComponentModel.DataAnnotations;

namespace AgencyCursor.WebApp.Models;

public class InterpreterResponse
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public Request Request { get; set; } = null!;
    public int InterpreterId { get; set; }
    public Interpreter Interpreter { get; set; } = null!;
    [Required] public string ResponseStatus { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime RespondedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
