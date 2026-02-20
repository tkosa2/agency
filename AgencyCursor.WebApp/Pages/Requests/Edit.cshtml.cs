using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;

namespace AgencyCursor.WebApp.Pages.Requests;

public class EditModel : PageModel
{
    private readonly AgencyDbContext _db;
    public EditModel(AgencyDbContext db) => _db = db;
    [BindProperty] public new Models.Request Request { get; set; } = null!;
    public SelectList RequestorList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var r = await _db.Requests.FindAsync(id);
        if (r == null) return NotFound();
        Request = r;
        RequestorList = new SelectList(await _db.Requestors.ToListAsync(), "Id", "FullName");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        RequestorList = new SelectList(await _db.Requestors.ToListAsync(), "Id", "FullName");
        if (!ModelState.IsValid) return Page();
        _db.Attach(Request).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
