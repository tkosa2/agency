using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;

namespace AgencyCursor.WebApp.Pages.Requests;

public class CreateModel : PageModel
{
    private readonly AgencyDbContext _db;
    public CreateModel(AgencyDbContext db) => _db = db;
    [BindProperty] public new Models.Request Request { get; set; } = new();
    public SelectList RequestorList { get; set; } = null!;

    public async Task OnGetAsync()
    {
        RequestorList = new SelectList(await _db.Requestors.ToListAsync(), "Id", "FullName");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        RequestorList = new SelectList(await _db.Requestors.ToListAsync(), "Id", "FullName");
        if (!ModelState.IsValid) return Page();
        _db.Requests.Add(Request);
        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
