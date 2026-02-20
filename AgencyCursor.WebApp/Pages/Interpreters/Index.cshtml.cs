using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Interpreters;

public class IndexModel : PageModel
{
    private readonly AgencyDbContext _db;
    public IndexModel(AgencyDbContext db) => _db = db;
    public List<Interpreter> Interpreters { get; set; } = new();
    public async Task OnGetAsync() => Interpreters = await _db.Interpreters.OrderBy(i => i.LastName).ToListAsync();
}
