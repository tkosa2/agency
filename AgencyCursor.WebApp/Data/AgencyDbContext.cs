using Microsoft.EntityFrameworkCore;
using AgencyCursor.WebApp.Models;

namespace AgencyCursor.WebApp.Data;

public class AgencyDbContext : DbContext
{
    public AgencyDbContext(DbContextOptions<AgencyDbContext> options) : base(options) { }

    public DbSet<Requestor> Requestors => Set<Requestor>();
    public DbSet<Interpreter> Interpreters => Set<Interpreter>();
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<InterpreterResponse> InterpreterResponses => Set<InterpreterResponse>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InterpreterEmailLog> InterpreterEmailLogs => Set<InterpreterEmailLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Invoice>()
            .Property(i => i.Amount)
            .HasColumnType("decimal(18,2)");
    }
}
