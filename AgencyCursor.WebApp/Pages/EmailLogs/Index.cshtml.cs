using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.EmailLogs;

public class IndexModel : PageModel
{
    private readonly AgencyDbContext _db;
    public IndexModel(AgencyDbContext db) => _db = db;
    public List<InterpreterEmailLog> Logs { get; set; } = new();
    public async Task OnGetAsync() =>
        Logs = await _db.InterpreterEmailLogs.Include(l => l.Interpreter).Include(l => l.Request).OrderByDescending(l => l.SentAt).ToListAsync();
}
