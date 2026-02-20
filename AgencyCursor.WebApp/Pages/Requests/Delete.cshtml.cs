using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;

namespace AgencyCursor.WebApp.Pages.Requests;

public class DeleteModel : PageModel
{
    private readonly AgencyDbContext _db;
    public DeleteModel(AgencyDbContext db) => _db = db;
    public new Models.Request Request { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var r = await _db.Requests.Include(x => x.Requestor).FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();
        Request = r;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var r = await _db.Requests.FindAsync(id);
        if (r != null) { _db.Requests.Remove(r); await _db.SaveChangesAsync(); }
        return RedirectToPage("Index");
    }
}
