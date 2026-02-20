using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Appointments;

public class IndexModel : PageModel
{
    private readonly AgencyDbContext _db;
    public IndexModel(AgencyDbContext db) => _db = db;
    public List<Appointment> Appointments { get; set; } = new();
    public async Task OnGetAsync() =>
        Appointments = await _db.Appointments.Include(a => a.Interpreter).Include(a => a.Request).OrderByDescending(a => a.AppointmentDate).ToListAsync();
}
