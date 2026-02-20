using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Invoices;

public class CreateModel : PageModel
{
    private readonly AgencyDbContext _db;
    public CreateModel(AgencyDbContext db) => _db = db;
    [BindProperty] public Invoice Invoice { get; set; } = new();
    public SelectList AppointmentList { get; set; } = null!;

    public async Task OnGetAsync() =>
        AppointmentList = new SelectList(await _db.Appointments.ToListAsync(), "Id", "Id");

    public async Task<IActionResult> OnPostAsync()
    {
        AppointmentList = new SelectList(await _db.Appointments.ToListAsync(), "Id", "Id");
        if (!ModelState.IsValid) return Page();
        _db.Invoices.Add(Invoice);
        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
