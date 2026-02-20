using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Invoices;

public class DeleteModel : PageModel
{
    private readonly AgencyDbContext _db;
    public DeleteModel(AgencyDbContext db) => _db = db;
    public Invoice Invoice { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var inv = await _db.Invoices.FindAsync(id);
        if (inv == null) return NotFound();
        Invoice = inv;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var inv = await _db.Invoices.FindAsync(id);
        if (inv != null) { _db.Invoices.Remove(inv); await _db.SaveChangesAsync(); }
        return RedirectToPage("Index");
    }
}
