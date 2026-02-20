using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AgencyCursor.WebApp.Models;
using AgencyCursor.WebApp.Services;

namespace AgencyCursor.WebApp.Pages.Interpreters;

public class SearchDirectoryModel : PageModel
{
    private readonly IDirectorySearchService _directorySearch;

    public SearchDirectoryModel(IDirectorySearchService directorySearch)
    {
        _directorySearch = directorySearch;
    }

    [BindProperty(SupportsGet = true)] public string? FirstName { get; set; }
    [BindProperty(SupportsGet = true)] public string? LastName { get; set; }
    [BindProperty(SupportsGet = true)] public string? Certificate { get; set; }
    [BindProperty(SupportsGet = true)] public string? Specialty { get; set; }
    [BindProperty(SupportsGet = true)] public string? City { get; set; }
    [BindProperty(SupportsGet = true)] public string? State { get; set; }
    [BindProperty(SupportsGet = true)] public string? Zip { get; set; }

    public List<DirectoryInterpreter> Results { get; set; } = new();
    public bool Searched { get; set; }

    public async Task OnGetAsync()
    {
        bool hasFilter = !string.IsNullOrWhiteSpace(FirstName)
            || !string.IsNullOrWhiteSpace(LastName)
            || !string.IsNullOrWhiteSpace(Certificate)
            || !string.IsNullOrWhiteSpace(Specialty)
            || !string.IsNullOrWhiteSpace(City)
            || !string.IsNullOrWhiteSpace(State)
            || !string.IsNullOrWhiteSpace(Zip);

        if (hasFilter)
        {
            Searched = true;
            Results = await _directorySearch.SearchAsync(FirstName, LastName, Certificate, Specialty, City, State, Zip);
        }
    }
}
