using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AgencyCursor.WebApp.Pages.Request;

public class ConfirmationModel : PageModel
{
    public int RequestId { get; set; }
    public void OnGet(int id) => RequestId = id;
}
