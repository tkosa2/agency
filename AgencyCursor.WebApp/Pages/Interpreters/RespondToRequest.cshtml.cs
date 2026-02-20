using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Interpreters;

public class RespondToRequestModel : PageModel
{
    private readonly AgencyDbContext _db;
    public RespondToRequestModel(AgencyDbContext db) => _db = db;
    public new Models.Request Request { get; set; } = null!;
    public InterpreterResponse? ExistingResponse { get; set; }
    [BindProperty, Required] public string ResponseStatus { get; set; } = string.Empty;
    [BindProperty] public string? Notes { get; set; }

    public async Task<IActionResult> OnGetAsync(int requestId, int interpreterId)
    {
        var r = await _db.Requests.FindAsync(requestId);
        if (r == null) return NotFound();
        Request = r;
        ExistingResponse = await _db.InterpreterResponses
            .FirstOrDefaultAsync(x => x.RequestId == requestId && x.InterpreterId == interpreterId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int requestId, int interpreterId)
    {
        var r = await _db.Requests.FindAsync(requestId);
        if (r == null) return NotFound();
        Request = r;
        if (!ModelState.IsValid) return Page();

        var existing = await _db.InterpreterResponses
            .FirstOrDefaultAsync(x => x.RequestId == requestId && x.InterpreterId == interpreterId);
        if (existing != null)
        {
            existing.ResponseStatus = ResponseStatus;
            existing.Notes = Notes;
            existing.RespondedAt = DateTime.UtcNow;
        }
        else
        {
            _db.InterpreterResponses.Add(new InterpreterResponse
            {
                RequestId = requestId,
                InterpreterId = interpreterId,
                ResponseStatus = ResponseStatus,
                Notes = Notes
            });
        }
        await _db.SaveChangesAsync();
        return RedirectToPage("/Interpreters/ThankYou");
    }
}
