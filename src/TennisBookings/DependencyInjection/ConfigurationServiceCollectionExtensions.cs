using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace TennisBookings.DependencyInjection;

// Demostrates Binding and Option Pattern configuration

public static class ConfigurationServiceCollectionExtensions
{
	public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
	{
		// Binding
		services.Configure<ClubConfiguration>(config.GetSection("ClubSettings"));
		services.Configure<BookingConfiguration>(config.GetSection("CourtBookings"));
		services.Configure<MembershipConfiguration>(config.GetSection("Membership"));
		services.Configure<ContentConfiguration>(config.GetSection("Content"));

		// Registration for Option forwarding base on IContentConfiguration
		services.AddSingleton<IContentConfiguration>(sp =>
			sp.GetRequiredService<IOptions<ContentConfiguration>>().Value);

		//Registration Option Pattern
		services.TryAddSingleton<IFeaturesConfiguration>(sp =>
			sp.GetRequiredService<IOptions<FeaturesConfiguration>>().Value); // forwarding via implementation factory

		services.TryAddSingleton<IBookingConfiguration>(sp =>
			sp.GetRequiredService<IOptions<BookingConfiguration>>().Value); // forwarding via implementation factory

		services.TryAddSingleton<IClubConfiguration>(sp =>
			sp.GetRequiredService<IOptions<ClubConfiguration>>().Value); // forwarding via implementation factory

		return services;
	}
}
