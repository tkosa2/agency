using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Data;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Pages.Request;

public class IndexModel : PageModel
{
    private readonly AgencyDbContext _db;
    public IndexModel(AgencyDbContext db) => _db = db;

    [BindProperty, Required] public string FirstName { get; set; } = string.Empty;
    [BindProperty, Required] public string LastName { get; set; } = string.Empty;
    [BindProperty, Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [BindProperty, Required] public string Phone { get; set; } = string.Empty;
    [BindProperty] public string? RequestorNotes { get; set; }
    [BindProperty, Required] public DateTime AppointmentDate { get; set; } = DateTime.Today.AddDays(1);
    [BindProperty, Required] public string StartTime { get; set; } = "09:00";
    [BindProperty, Required] public string EndTime { get; set; } = "11:00";
    [BindProperty, Required] public string IndividualType { get; set; } = string.Empty;
    [BindProperty] public int NumberOfDeafIndividuals { get; set; } = 1;
    [BindProperty, Required] public string ServiceType { get; set; } = string.Empty;
    [BindProperty] public string? ServiceTypeOther { get; set; }
    [BindProperty, Required] public string Mode { get; set; } = string.Empty;
    [BindProperty] public string? Address { get; set; }
    [BindProperty] public string? Address2 { get; set; }
    [BindProperty] public string? City { get; set; }
    [BindProperty] public string? State { get; set; }
    [BindProperty] public string? ZipCode { get; set; }
    [BindProperty] public string? VirtualMeetingLink { get; set; }
    [BindProperty] public string? GenderPreference { get; set; }
    [BindProperty] public string? PreferredInterpreter { get; set; }
    [BindProperty] public string? SpecialRequirements { get; set; }
    [BindProperty] public string? AdditionalInfo { get; set; }
    [BindProperty, Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the terms.")]
    public bool AgreeToTerms { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var requestor = await _db.Requestors.FirstOrDefaultAsync(r => r.Email == Email);
        if (requestor == null)
        {
            requestor = new Requestor { FirstName = FirstName, LastName = LastName, Email = Email, Phone = Phone, Notes = RequestorNotes };
            _db.Requestors.Add(requestor);
            await _db.SaveChangesAsync();
        }

        var request = new Models.Request
        {
            RequestorId = requestor.Id,
            AppointmentDate = AppointmentDate,
            StartTime = TimeSpan.Parse(StartTime),
            EndTime = TimeSpan.Parse(EndTime),
            IndividualType = IndividualType,
            NumberOfDeafIndividuals = NumberOfDeafIndividuals,
            ServiceType = ServiceType,
            ServiceTypeOther = ServiceTypeOther,
            Mode = Mode,
            Address = Address,
            Address2 = Address2,
            City = City,
            State = State,
            ZipCode = ZipCode,
            VirtualMeetingLink = VirtualMeetingLink,
            GenderPreference = GenderPreference,
            PreferredInterpreter = PreferredInterpreter,
            SpecialRequirements = SpecialRequirements,
            AdditionalInfo = AdditionalInfo,
            Status = "New Request"
        };
        _db.Requests.Add(request);
        await _db.SaveChangesAsync();

        return RedirectToPage("/Request/Confirmation", new { id = request.Id });
    }
}
