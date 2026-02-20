using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Invoices;

public class DetailsModel : PageModel
{
    private readonly AgencyDbContext _db;
    public DetailsModel(AgencyDbContext db) => _db = db;
    public Invoice Invoice { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var inv = await _db.Invoices.FindAsync(id);
        if (inv == null) return NotFound();
        Invoice = inv;
        return Page();
    }
}
