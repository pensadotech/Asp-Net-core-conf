using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TennisBookings.DependencyInjection;

// ref: https://medium.com/@ZombieCodeKill/dependency-injection-in-asp-net-core-52b7e78bb4fd

public static class NotificationsServiceCollectionExtensions
{
	public static IServiceCollection AddNotifications(this IServiceCollection services)
	{
		services.TryAddSingleton<EmailNotificationService>();
		services.TryAddSingleton<SmsNotificationService>();

		services.AddSingleton<INotificationService>(sp =>
			new CompositeNotificationService(
				new INotificationService[]
				{
					sp.GetRequiredService<EmailNotificationService>(),
					sp.GetRequiredService<SmsNotificationService>()
				})); // composite pattern

		return services;
	}
}
