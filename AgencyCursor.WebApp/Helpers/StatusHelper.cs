namespace AgencyCursor.WebApp.Helpers;

public static class StatusHelper
{
    public static string GetBadgeClass(string? status) => status switch
    {
        "New Request" or "Reviewed" or "Approved" => "bg-warning text-dark",
        "Broadcasted" => "bg-primary",
        "Assigned" or "Confirmed" => "bg-info text-dark",
        "Completed" or "Cancelled<48h" => "bg-warning",
        "Cancelled>48h" => "bg-danger",
        "Paid" => "bg-success",
        "Pending" => "bg-secondary",
        "Yes" => "bg-success",
        "No" => "bg-danger",
        "Maybe" => "bg-warning text-dark",
        _ => "bg-secondary"
    };
}
