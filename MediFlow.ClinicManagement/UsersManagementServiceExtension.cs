using FluentValidation;
using MediatR;
using MediFlow.ClinicManagement.Authorization;
using MediFlow.ClinicManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernal;
using System.Reflection;

namespace MediFlow.ClinicManagement;

public static class UsersManagementServiceExtension
{
	public static IServiceCollection AddUsersManagementServices(
		this IServiceCollection services,
		IConfigurationManager config)
	{
		var connectionString = config.GetConnectionString("UsersManagement");
		var currentAssembly = Assembly.GetExecutingAssembly();

		services.AddDbContext<UsersManagementDbContext>(options => options.UseSqlServer(connectionString));

		services.AddHttpContextAccessor();
		services.AddScoped<IAuthorizationHandler, MustOwnClinicHandler>();

		services.AddAuthorizationBuilder()
					.AddPolicy("MustOwnClinic", (options) => {
						options.RequireAuthenticatedUser();
						options.RequireClaim("role", "clinic");
						options.AddRequirements(new MustOwnClinicRequirement());
					});

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
