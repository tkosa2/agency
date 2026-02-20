using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Appointments;

public class DeleteModel : PageModel
{
    private readonly AgencyDbContext _db;
    public DeleteModel(AgencyDbContext db) => _db = db;
    public Appointment Appointment { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var a = await _db.Appointments.FindAsync(id);
        if (a == null) return NotFound();
        Appointment = a;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var a = await _db.Appointments.FindAsync(id);
        if (a != null) { _db.Appointments.Remove(a); await _db.SaveChangesAsync(); }
        return RedirectToPage("Index");
    }
}
