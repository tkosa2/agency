using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;

namespace AgencyCursor.WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly AgencyDbContext _db;
    public IndexModel(AgencyDbContext db) => _db = db;

    public int RequestorCount { get; set; }
    public int InterpreterCount { get; set; }
    public int RequestCount { get; set; }
    public int AppointmentCount { get; set; }
    public int InvoiceCount { get; set; }
    public int NewRequestCount { get; set; }

    public async Task OnGetAsync()
    {
        RequestorCount = await _db.Requestors.CountAsync();
        InterpreterCount = await _db.Interpreters.CountAsync();
        RequestCount = await _db.Requests.CountAsync();
        AppointmentCount = await _db.Appointments.CountAsync();
        InvoiceCount = await _db.Invoices.CountAsync();
        NewRequestCount = await _db.Requests.CountAsync(r => r.Status == "New Request");
    }
}
