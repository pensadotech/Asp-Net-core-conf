using Microsoft.Extensions.DependencyInjection.Extensions;
using TennisBookings.External;
using TennisBookings.Services.Weather;

namespace TennisBookings.DependencyInjection;

public static class WeatherServiceCollectionExtensions
{
	public static IServiceCollection AddWeatherForecasting(this IServiceCollection services,
		IConfiguration config)
	{
		// The configuration in here is to enable or disable funcionality for full weather forecasting
		// services or to use a disable feature.
		if (config.GetValue<bool>("Features:WeatherForecasting:EnableWeatherForecasting"))
		{
			// instace to add external API calls
			services.AddHttpClient<IWeatherApiClient, WeatherApiClient>();
			//services.TryAddSingleton<IWeatherForecaster, RandomWeatherForecaster>();

			// This connect to an API serivice using HTTP client 
			services.TryAddSingleton<IWeatherForecaster, WeatherForecaster>();

			// Decoreate is part of 'Scruto" NuGet, it is inteded for cases in which
			// the decorator partern is used. The order is important and the Decorator
			// must be defined after declaring the dependency, in this example IWeatherForecaster
			// defined by WeatherForecaster.
			// The CachedWeatherForecaster is implementing IWeatherForecaster but adding chaching
			// funcitonality. Teh DI engine wil know that WeatherForecaster is required 
			// Ref: https://andrewlock.net/adding-decorated-classes-to-the-asp.net-core-di-container-using-scrutor/
			//      https://ardalis.com/building-a-cachedrepository-in-aspnet-core/
			services.Decorate<IWeatherForecaster, CachedWeatherForecaster>();
		}
		else
		{
			services.TryAddSingleton<IWeatherForecaster, DisabledWeatherForecaster>();
		}

		return services;
	}
}
