using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Appointments;

public class CreateModel : PageModel
{
    private readonly AgencyDbContext _db;
    public CreateModel(AgencyDbContext db) => _db = db;
    [BindProperty] public Appointment Appointment { get; set; } = new();
    public SelectList RequestList { get; set; } = null!;
    public SelectList InterpreterList { get; set; } = null!;

    public async Task OnGetAsync()
    {
        RequestList = new SelectList(await _db.Requests.Include(r => r.Requestor).ToListAsync(), "Id", "Id");
        InterpreterList = new SelectList(await _db.Interpreters.ToListAsync(), "Id", "FullName");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        RequestList = new SelectList(await _db.Requests.Include(r => r.Requestor).ToListAsync(), "Id", "Id");
        InterpreterList = new SelectList(await _db.Interpreters.ToListAsync(), "Id", "FullName");
        if (!ModelState.IsValid) return Page();
        _db.Appointments.Add(Appointment);
        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
