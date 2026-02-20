namespace AgencyCursor.WebApp.Models;

public class DirectoryInterpreter
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Category { get; set; }
    public string? FreelanceStatus { get; set; }
    public List<string> Certificates { get; set; } = new();
    public List<string> Specialties { get; set; } = new();
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string CertificatesSummary => Certificates.Count > 0 ? string.Join(", ", Certificates) : string.Empty;
    public string SpecialtiesSummary => Specialties.Count > 0 ? string.Join(", ", Specialties) : string.Empty;
}
