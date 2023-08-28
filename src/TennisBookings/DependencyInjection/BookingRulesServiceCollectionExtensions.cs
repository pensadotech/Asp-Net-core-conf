// NOTE: To scan services that implement simialr interface
//       it is requierd to use NuGet "Scrutor" 

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TennisBookings.DependencyInjection;

public static class BookingRulesServiceCollectionExtensions
{
	public static IServiceCollection AddBookingRules(this IServiceCollection services)
	{
		// Scrutor assembly scanning for all services associated with ICourtBookingRule
		services.Scan(scan => scan
			.FromAssemblyOf<ICourtBookingRule>()
				.AddClasses(classes => classes.AssignableTo<IScopedCourtBookingRule>())
					.As<ICourtBookingRule>()
					.WithScopedLifetime()
				.AddClasses(classes => classes.AssignableTo<ISingletonCourtBookingRule>())
					.As<ICourtBookingRule>()
					.WithSingletonLifetime());

		services.TryAddScoped<IBookingRuleProcessor, BookingRuleProcessor>();

		return services;
	}
}
