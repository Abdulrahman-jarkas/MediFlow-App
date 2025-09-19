using MediFlow.DoctorManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernal;
using System.Reflection;
using FluentValidation;

namespace MediFlow.DoctorManagement;

public static class DoctorManagementServiceExtension
{
	public static IServiceCollection AddDoctorManagementServices(
		this IServiceCollection services,
		IConfigurationManager config)
	{
		var connectionString = config.GetConnectionString("DoctorsManagement");
		var currentAssembly = Assembly.GetExecutingAssembly();

		services.AddDbContext<DoctorDbContext>(options => options.UseSqlServer(connectionString));

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
}
