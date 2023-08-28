namespace TennisBookings.Shared.Weather;

// Init as setter in properties - this requires NuGet Microsoft.Extensions.Caching.Abstractions
// the intention is to allow a single time initialization, and blocked for that point forward
// This functionality is included on higher version that .NET 6
// REf: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/init

public class Temperature
{
    public Temperature(float min, float max)
    {
        Min = min;
        Max = max;
    }

    public float Min { get; init; }
    public float Max { get; init; }
}
