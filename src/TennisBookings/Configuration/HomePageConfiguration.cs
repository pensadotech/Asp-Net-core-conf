using System.ComponentModel.DataAnnotations;

namespace TennisBookings.Configuration;

public class HomePageConfiguration
{
	public bool EnableGreeting { get; set; }
	public bool EnableWeatherForecast { get; set; }

	// use data annotation attributes when registering the services with IOptions and ValidateDataAnnotations()
	// [Required(ErrorMessage = "A title is requierd for the weather forecast section.")]
	public string ForecastSectionTitle { get; set; } = string.Empty;
}
