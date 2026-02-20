using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Appointments;

public class EditModel : PageModel
{
    private readonly AgencyDbContext _db;
    public EditModel(AgencyDbContext db) => _db = db;
    [BindProperty] public Appointment Appointment { get; set; } = null!;
    public SelectList RequestList { get; set; } = null!;
    public SelectList InterpreterList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var a = await _db.Appointments.FindAsync(id);
        if (a == null) return NotFound();
        Appointment = a;
        RequestList = new SelectList(await _db.Requests.ToListAsync(), "Id", "Id");
        InterpreterList = new SelectList(await _db.Interpreters.ToListAsync(), "Id", "FullName");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        RequestList = new SelectList(await _db.Requests.ToListAsync(), "Id", "Id");
        InterpreterList = new SelectList(await _db.Interpreters.ToListAsync(), "Id", "FullName");
        if (!ModelState.IsValid) return Page();
        _db.Attach(Appointment).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
