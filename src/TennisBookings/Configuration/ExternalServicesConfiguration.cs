namespace TennisBookings.Configuration;

public class ExternalServicesConfiguration
{
	// Constants that represent each configuration point
	// and can be used to refer the Named Option to use
	// and avoid having to write these strings accross
	// the code
	public const string WeatherApi = "WeatherApi";
	public const string ProductsApi = "ProductsApi";

	// Common properties for two seto of configuration
	// points, in this case WeatherApi and ProducstApi
	// Configurations Named options will be used to
	// serve both with this common class
	public string Url { get; set; } = string.Empty;
	public int MinsToCache { get; set; } = 10;

	// API key 
	public string ApiKey { get; set; } = string.Empty;
}
