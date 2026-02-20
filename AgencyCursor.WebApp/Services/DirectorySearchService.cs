using Microsoft.Data.Sqlite;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Services;

public class DirectorySearchService : IDirectorySearchService
{
    private readonly string _connectionString;

    public DirectorySearchService(IConfiguration configuration, IWebHostEnvironment env)
    {
        var relativePath = configuration["DirectoryDb:Path"] ?? "directory/rid_interpreters.db";
        var fullPath = Path.IsPathRooted(relativePath)
            ? relativePath
            : Path.Combine(env.ContentRootPath, relativePath);
        _connectionString = $"Data Source={fullPath};Mode=ReadOnly";
    }

    public async Task<List<DirectoryInterpreter>> SearchAsync(
        string? firstName,
        string? lastName,
        string? certificate,
        string? specialty,
        string? city,
        string? state,
        string? zip)
    {
        // At least one filter is required to avoid loading all 10k+ records at once
        bool hasFilter = !string.IsNullOrWhiteSpace(firstName)
            || !string.IsNullOrWhiteSpace(lastName)
            || !string.IsNullOrWhiteSpace(certificate)
            || !string.IsNullOrWhiteSpace(specialty)
            || !string.IsNullOrWhiteSpace(city)
            || !string.IsNullOrWhiteSpace(state)
            || !string.IsNullOrWhiteSpace(zip);

        if (!hasFilter) return new List<DirectoryInterpreter>();

        var conditions = new List<string>();
        var parameters = new Dictionary<string, object?>();

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            conditions.Add("LOWER(i.first_name) LIKE @firstName");
            parameters["@firstName"] = $"%{firstName.Trim().ToLowerInvariant()}%";
        }
        if (!string.IsNullOrWhiteSpace(lastName))
        {
            conditions.Add("LOWER(i.last_name) LIKE @lastName");
            parameters["@lastName"] = $"%{lastName.Trim().ToLowerInvariant()}%";
        }
        if (!string.IsNullOrWhiteSpace(city))
        {
            conditions.Add("LOWER(i.city) LIKE @city");
            parameters["@city"] = $"%{city.Trim().ToLowerInvariant()}%";
        }
        if (!string.IsNullOrWhiteSpace(state))
        {
            conditions.Add("LOWER(i.state) LIKE @state");
            parameters["@state"] = $"%{state.Trim().ToLowerInvariant()}%";
        }
        if (!string.IsNullOrWhiteSpace(zip))
        {
            conditions.Add("i.zip_code LIKE @zip");
            parameters["@zip"] = $"{zip.Trim()}%";
        }
        if (!string.IsNullOrWhiteSpace(certificate))
        {
            conditions.Add("EXISTS (SELECT 1 FROM interpreter_certificates ic WHERE ic.interpreter_id = i.id AND LOWER(ic.certificate) LIKE @certificate)");
            parameters["@certificate"] = $"%{certificate.Trim().ToLowerInvariant()}%";
        }
        if (!string.IsNullOrWhiteSpace(specialty))
        {
            conditions.Add("EXISTS (SELECT 1 FROM interpreter_specialties isp WHERE isp.interpreter_id = i.id AND LOWER(isp.specialty) LIKE @specialty)");
            parameters["@specialty"] = $"%{specialty.Trim().ToLowerInvariant()}%";
        }

        var whereClause = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : string.Empty;
        var sql = $"SELECT i.id, i.first_name, i.last_name, i.email, i.city, i.state, i.zip_code, i.category, i.freelance_status FROM interpreters i {whereClause} ORDER BY i.last_name, i.first_name LIMIT 100";

        var interpreters = new Dictionary<int, DirectoryInterpreter>();

        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using var cmd = connection.CreateCommand();
        cmd.CommandText = sql;
        foreach (var (key, value) in parameters)
        {
            cmd.Parameters.AddWithValue(key, value ?? DBNull.Value);
        }

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32(0);
            var interp = new DirectoryInterpreter
            {
                Id = id,
                FirstName = reader.IsDBNull(1) ? null : reader.GetString(1),
                LastName = reader.IsDBNull(2) ? null : reader.GetString(2),
                Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                City = reader.IsDBNull(4) ? null : reader.GetString(4),
                State = reader.IsDBNull(5) ? null : reader.GetString(5),
                ZipCode = reader.IsDBNull(6) ? null : reader.GetString(6),
                Category = reader.IsDBNull(7) ? null : reader.GetString(7),
                FreelanceStatus = reader.IsDBNull(8) ? null : reader.GetString(8),
            };
            interpreters[id] = interp;
        }

        if (interpreters.Count == 0) return new List<DirectoryInterpreter>();

        // IDs come from reader.GetInt32 so they are safe integers,
        // but we explicitly cast to ensure no injection risk.
        var ids = string.Join(",", interpreters.Keys.Select(k => k.ToString()));

        // Load certificates
        await using var certCmd = connection.CreateCommand();
        certCmd.CommandText = $"SELECT interpreter_id, certificate FROM interpreter_certificates WHERE interpreter_id IN ({ids})";
        await using var certReader = await certCmd.ExecuteReaderAsync();
        while (await certReader.ReadAsync())
        {
            var id = certReader.GetInt32(0);
            if (interpreters.TryGetValue(id, out var interp))
                interp.Certificates.Add(certReader.GetString(1));
        }

        // Load specialties
        await using var specCmd = connection.CreateCommand();
        specCmd.CommandText = $"SELECT interpreter_id, specialty FROM interpreter_specialties WHERE interpreter_id IN ({ids})";
        await using var specReader = await specCmd.ExecuteReaderAsync();
        while (await specReader.ReadAsync())
        {
            var id = specReader.GetInt32(0);
            if (interpreters.TryGetValue(id, out var interp))
                interp.Specialties.Add(specReader.GetString(1));
        }

        return interpreters.Values.ToList();
    }

    public async Task<DirectoryInterpreter?> GetByIdAsync(int id)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT id, first_name, last_name, email, city, state, zip_code, category, freelance_status FROM interpreters WHERE id = @id";
        cmd.Parameters.AddWithValue("@id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        var interp = new DirectoryInterpreter
        {
            Id = reader.GetInt32(0),
            FirstName = reader.IsDBNull(1) ? null : reader.GetString(1),
            LastName = reader.IsDBNull(2) ? null : reader.GetString(2),
            Email = reader.IsDBNull(3) ? null : reader.GetString(3),
            City = reader.IsDBNull(4) ? null : reader.GetString(4),
            State = reader.IsDBNull(5) ? null : reader.GetString(5),
            ZipCode = reader.IsDBNull(6) ? null : reader.GetString(6),
            Category = reader.IsDBNull(7) ? null : reader.GetString(7),
            FreelanceStatus = reader.IsDBNull(8) ? null : reader.GetString(8),
        };

        await using var certCmd = connection.CreateCommand();
        certCmd.CommandText = "SELECT certificate FROM interpreter_certificates WHERE interpreter_id = @id";
        certCmd.Parameters.AddWithValue("@id", id);
        await using var certReader = await certCmd.ExecuteReaderAsync();
        while (await certReader.ReadAsync())
            interp.Certificates.Add(certReader.GetString(0));

        await using var specCmd = connection.CreateCommand();
        specCmd.CommandText = "SELECT specialty FROM interpreter_specialties WHERE interpreter_id = @id";
        specCmd.Parameters.AddWithValue("@id", id);
        await using var specReader = await specCmd.ExecuteReaderAsync();
        while (await specReader.ReadAsync())
            interp.Specialties.Add(specReader.GetString(0));

        return interp;
    }
}
