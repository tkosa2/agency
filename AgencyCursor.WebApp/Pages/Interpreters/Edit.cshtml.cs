using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Interpreters;

public class EditModel : PageModel
{
    private readonly AgencyDbContext _db;
    public EditModel(AgencyDbContext db) => _db = db;
    [BindProperty] public Interpreter Interpreter { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var i = await _db.Interpreters.FindAsync(id);
        if (i == null) return NotFound();
        Interpreter = i;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        _db.Attach(Interpreter).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
