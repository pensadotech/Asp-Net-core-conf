// New way to indicate that the functinality belong to thsi name space
namespace TennisBookings.Shared.Weather;

// Init as setter in properties - this requires NuGet Microsoft.Extensions.Caching.Abstractions
// the intention is to allow a single time initialization, and blocked for that point forward
// This functionality is included on higher version that .NET 6
// REf: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/init
public class WeatherCondition
{
    public string Summary { get; init; } = "Unknown";
    public Wind Wind { get; init; } = new Wind(0, 0);
    public Temperature Temperature { get; init; } = new Temperature(0, 0);
}
