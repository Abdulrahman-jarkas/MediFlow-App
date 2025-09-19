using FluentValidation;
using MediatR;
using MediFlow.Schedule.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernal;
using System;
using System.Reflection;

namespace MediFlow.ClinicManagement;

public static class ScheduleManagementServiceExtension
{
	public static IServiceCollection AddScheduleServices(
		this IServiceCollection services,
		IConfigurationManager config)
	{
		var connectionString = config.GetConnectionString("Schedule");
		var currentAssembly = Assembly.GetExecutingAssembly();

		services.AddDbContext<ScheduleDbContext>(options => options.UseSqlServer(connectionString));

		services.AddHttpContextAccessor();

		services.RegisterSlices(currentAssembly);

		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssemblies(currentAssembly)
				.AddOpenBehavior(typeof(ValidationBehavior<,>));
		});

		services.AddValidatorsFromAssembly(assembly: currentAssembly, includeInternalTypes: true);

		return services;
	}

	public static async Task SeedScheduleDataAsync(this WebApplication app)
	{
		using (var scope = app.Services.CreateScope())
		{
			var db = scope.ServiceProvider.GetRequiredService<ScheduleDbContext>();
			await SeedData.SeedInitialDataAsync(db);
		}
	}
}
