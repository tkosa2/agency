using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Appointments;

public class DetailsModel : PageModel
{
    private readonly AgencyDbContext _db;
    public DetailsModel(AgencyDbContext db) => _db = db;
    public Appointment Appointment { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var a = await _db.Appointments
            .Include(x => x.Interpreter)
            .Include(x => x.Request).ThenInclude(x => x.Requestor)
            .Include(x => x.Invoices)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (a == null) return NotFound();
        Appointment = a;
        return Page();
    }
}
