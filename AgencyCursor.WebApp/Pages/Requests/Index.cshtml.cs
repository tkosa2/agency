using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;

namespace AgencyCursor.WebApp.Pages.Requests;

public class IndexModel : PageModel
{
    private readonly AgencyDbContext _db;
    public IndexModel(AgencyDbContext db) => _db = db;
    public List<Models.Request> Requests { get; set; } = new();
    public string? StatusFilter { get; set; }

    public async Task OnGetAsync(string? status)
    {
        StatusFilter = status;
        var query = _db.Requests.Include(r => r.Requestor).AsQueryable();
        if (!string.IsNullOrEmpty(status))
            query = query.Where(r => r.Status == status);
        Requests = await query.OrderByDescending(r => r.CreatedAt).ToListAsync();
    }
}
