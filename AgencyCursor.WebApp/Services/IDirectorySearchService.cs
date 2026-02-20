using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Services;

public interface IDirectorySearchService
{
    Task<List<DirectoryInterpreter>> SearchAsync(
        string? firstName,
        string? lastName,
        string? certificate,
        string? specialty,
        string? city,
        string? state,
        string? zip);

    Task<DirectoryInterpreter?> GetByIdAsync(int id);
}
