using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Invoices;

public class IndexModel : PageModel
{
    private readonly AgencyDbContext _db;
    public IndexModel(AgencyDbContext db) => _db = db;
    public List<Invoice> Invoices { get; set; } = new();
    public async Task OnGetAsync() => Invoices = await _db.Invoices.Include(i => i.Appointment).OrderByDescending(i => i.CreatedDate).ToListAsync();
}
