using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Interpreters;

public class DetailsModel : PageModel
{
    private readonly AgencyDbContext _db;
    public DetailsModel(AgencyDbContext db) => _db = db;
    public Interpreter Interpreter { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var i = await _db.Interpreters.Include(x => x.Appointments).FirstOrDefaultAsync(x => x.Id == id);
        if (i == null) return NotFound();
        Interpreter = i;
        return Page();
    }
}
