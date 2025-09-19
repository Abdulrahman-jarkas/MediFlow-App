using MediFlow.ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using SharedKernel;

namespace MediFlow.ClinicManagement.Data;

public class UsersManagementDbContext : DbContext
{
	internal DbSet<Clinic> Clinics { get; set; }
	internal DbSet<Medicine> Medicines { get; set; }
	internal DbSet<Equipment> Equipments { get; set; }
	internal DbSet<Room> Rooms { get; set; }
	internal DbSet<Doctor> Doctors { get; set; }

	private readonly IDomainEventDispatcher? _dispatcher;
	public UsersManagementDbContext(DbContextOptions<UsersManagementDbContext> options, IDomainEventDispatcher domainEventDispatcher) : base(options)
	{
		_dispatcher = domainEventDispatcher;
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("UsersManagement");

		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	protected override void ConfigureConventions(
	  ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder.Properties<decimal>()
		  .HavePrecision(18, 6);
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
	{
		var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		// ignore events if no dispatcher provided
		if (_dispatcher == null) return result;

		// dispatch events only if save was successful
		var entitiesWithEvents = ChangeTracker.Entries<IHaveDomainEvents>()
			.Select(e => e.Entity)
			.Where(e => e.DomainEvents.Any())
		.ToArray();

		await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

		return result;
	}
}
