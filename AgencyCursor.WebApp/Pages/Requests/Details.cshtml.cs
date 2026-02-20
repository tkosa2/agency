using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Services;

namespace AgencyCursor.WebApp.Pages.Requests;

public class DetailsModel : PageModel
{
    private readonly AgencyDbContext _db;
    private readonly IEmailService _emailService;
    public DetailsModel(AgencyDbContext db, IEmailService emailService) { _db = db; _emailService = emailService; }
    public new Models.Request Request { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var r = await _db.Requests
            .Include(x => x.Requestor)
            .Include(x => x.Responses).ThenInclude(x => x.Interpreter)
            .Include(x => x.EmailLogs).ThenInclude(x => x.Interpreter)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();
        Request = r;
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(int id, string status)
    {
        var r = await _db.Requests.FindAsync(id);
        if (r != null) { r.Status = status; await _db.SaveChangesAsync(); }
        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostBroadcastAsync(int id)
    {
        var r = await _db.Requests.FindAsync(id);
        if (r == null) return NotFound();
        var interpreters = await _db.Interpreters.ToListAsync();
        await _emailService.BroadcastToInterpretersAsync(r, interpreters);
        r.Status = "Broadcasted";
        await _db.SaveChangesAsync();
        return RedirectToPage(new { id });
    }
}
