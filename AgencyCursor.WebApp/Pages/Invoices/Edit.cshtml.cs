using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Invoices;

public class EditModel : PageModel
{
    private readonly AgencyDbContext _db;
    public EditModel(AgencyDbContext db) => _db = db;
    [BindProperty] public Invoice Invoice { get; set; } = null!;
    public SelectList AppointmentList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var inv = await _db.Invoices.FindAsync(id);
        if (inv == null) return NotFound();
        Invoice = inv;
        AppointmentList = new SelectList(await _db.Appointments.ToListAsync(), "Id", "Id");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        AppointmentList = new SelectList(await _db.Appointments.ToListAsync(), "Id", "Id");
        if (!ModelState.IsValid) return Page();
        _db.Attach(Invoice).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
