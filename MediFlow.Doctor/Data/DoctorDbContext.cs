using DoctorManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MediFlow.DoctorManagement.Data;

public class DoctorDbContext : DbContext
{
	internal DbSet<Doctor> Doctors { get; set; }

	public DoctorDbContext(DbContextOptions<DoctorDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("DoctorsManagement");

		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	protected override void ConfigureConventions(
	  ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder.Properties<decimal>()
		  .HavePrecision(18, 6);
	}
}
