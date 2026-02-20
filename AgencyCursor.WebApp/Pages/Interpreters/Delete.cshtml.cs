using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Interpreters;

public class DeleteModel : PageModel
{
    private readonly AgencyDbContext _db;
    public DeleteModel(AgencyDbContext db) => _db = db;
    public Interpreter Interpreter { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var i = await _db.Interpreters.FindAsync(id);
        if (i == null) return NotFound();
        Interpreter = i;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var i = await _db.Interpreters.FindAsync(id);
        if (i != null) { _db.Interpreters.Remove(i); await _db.SaveChangesAsync(); }
        return RedirectToPage("Index");
    }
}
