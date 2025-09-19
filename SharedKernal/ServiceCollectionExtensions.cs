using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SharedKernal;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterSlices(this IServiceCollection services, Assembly assembly)
	{
		// get slices 
		var slices = assembly.GetTypes().Where(t =>
			typeof(ISlice).IsAssignableFrom(t) &&
			t != typeof(ISlice) &&
			t.IsPublic &&
			!t.IsAbstract);

		// register them as singletons
		foreach (var slice in slices)
		{
			services.AddSingleton(typeof(ISlice), slice);
		}

		return services;
	}
}
