using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;
using AgencyCursor.WebApp.Services;

namespace AgencyCursor.WebApp.Pages.Interpreters;

public class RegisterFromDirectoryModel : PageModel
{
    private readonly IDirectorySearchService _directorySearch;
    private readonly AgencyDbContext _db;

    public RegisterFromDirectoryModel(IDirectorySearchService directorySearch, AgencyDbContext db)
    {
        _directorySearch = directorySearch;
        _db = db;
    }

    [BindProperty] public Interpreter Interpreter { get; set; } = new();
    public DirectoryInterpreter? DirectoryEntry { get; set; }

    // Query params used to build back-link to search results
    public string? SearchFirstName { get; set; }
    public string? SearchLastName { get; set; }
    public string? SearchCertificate { get; set; }
    public string? SearchSpecialty { get; set; }
    public string? SearchCity { get; set; }
    public string? SearchState { get; set; }
    public string? SearchZip { get; set; }

    public async Task<IActionResult> OnGetAsync(
        int directoryId,
        string? firstName,
        string? lastName,
        string? certificate,
        string? specialty,
        string? city,
        string? state,
        string? zip)
    {
        DirectoryEntry = await _directorySearch.GetByIdAsync(directoryId);
        if (DirectoryEntry == null) return NotFound();

        // Pre-populate interpreter form with directory data
        Interpreter.FirstName = DirectoryEntry.FirstName ?? string.Empty;
        Interpreter.LastName = DirectoryEntry.LastName ?? string.Empty;
        Interpreter.Email = DirectoryEntry.Email ?? string.Empty;
        Interpreter.Certification = DirectoryEntry.CertificatesSummary;
        Interpreter.Language = "ASL";
        Interpreter.Notes = BuildNotes(DirectoryEntry);

        SearchFirstName = firstName;
        SearchLastName = lastName;
        SearchCertificate = certificate;
        SearchSpecialty = specialty;
        SearchCity = city;
        SearchState = state;
        SearchZip = zip;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        _db.Interpreters.Add(Interpreter);
        await _db.SaveChangesAsync();
        return RedirectToPage("Details", new { id = Interpreter.Id });
    }

    private static string BuildNotes(DirectoryInterpreter entry)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(entry.State)) parts.Add($"State: {entry.State}");
        if (!string.IsNullOrWhiteSpace(entry.City)) parts.Add($"City: {entry.City}");
        if (!string.IsNullOrWhiteSpace(entry.ZipCode)) parts.Add($"Zip: {entry.ZipCode}");
        if (!string.IsNullOrWhiteSpace(entry.Category)) parts.Add($"Category: {entry.Category}");
        if (!string.IsNullOrWhiteSpace(entry.FreelanceStatus)) parts.Add($"Freelance: {entry.FreelanceStatus}");
        if (entry.Specialties.Count > 0) parts.Add($"Specialties: {entry.SpecialtiesSummary}");
        return string.Join(" | ", parts);
    }
}
