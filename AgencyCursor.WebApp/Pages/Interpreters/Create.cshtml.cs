using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Interpreters;

public class CreateModel : PageModel
{
    private readonly AgencyDbContext _db;
    public CreateModel(AgencyDbContext db) => _db = db;
    [BindProperty] public Interpreter Interpreter { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        _db.Interpreters.Add(Interpreter);
        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
