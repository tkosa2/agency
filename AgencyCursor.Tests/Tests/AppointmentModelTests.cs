using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.Tests.Tests;

public class AppointmentModelTests
{
    [Fact]
    public void AppointmentRequestNavigationHasValidateNeverAttribute()
    {
        var prop = typeof(Appointment).GetProperty(nameof(Appointment.Request));
        Assert.NotNull(prop);
        Assert.NotNull(prop!.GetCustomAttribute<ValidateNeverAttribute>());
    }

    [Fact]
    public void AppointmentInterpreterNavigationHasValidateNeverAttribute()
    {
        var prop = typeof(Appointment).GetProperty(nameof(Appointment.Interpreter));
        Assert.NotNull(prop);
        Assert.NotNull(prop!.GetCustomAttribute<ValidateNeverAttribute>());
    }

    [Fact]
    public void RequestRequestorNavigationHasValidateNeverAttribute()
    {
        var prop = typeof(Request).GetProperty(nameof(Request.Requestor));
        Assert.NotNull(prop);
        Assert.NotNull(prop!.GetCustomAttribute<ValidateNeverAttribute>());
    }

    [Fact]
    public void InvoiceAppointmentNavigationHasValidateNeverAttribute()
    {
        var prop = typeof(Invoice).GetProperty(nameof(Invoice.Appointment));
        Assert.NotNull(prop);
        Assert.NotNull(prop!.GetCustomAttribute<ValidateNeverAttribute>());
    }
}
