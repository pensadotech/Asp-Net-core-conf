using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TennisBookings.DependencyInjection;

public static class UnavailabilityServiceCollectionExtensions
{
	public static IServiceCollection AddCourtUnavailability(this IServiceCollection services)
	{
		services.TryAddEnumerable(new[]
		{
			// User service Descriptor to add services into the session. This is an
			// alternative to using the 'services' (e.g. services.TryAddScoped)
			ServiceDescriptor.Scoped<IUnavailabilityProvider, ClubClosedUnavailabilityProvider>(),
			ServiceDescriptor.Scoped<IUnavailabilityProvider, ClubClosedUnavailabilityProvider>(),
			ServiceDescriptor.Scoped<IUnavailabilityProvider, UpcomingHoursUnavailabilityProvider>(),
			ServiceDescriptor.Scoped<IUnavailabilityProvider, OutsideCourtUnavailabilityProvider>(),
			ServiceDescriptor.Scoped<IUnavailabilityProvider, CourtBookingUnavailabilityProvider>()
		}); // register multiple implementations manually

		return services;
	}
}
