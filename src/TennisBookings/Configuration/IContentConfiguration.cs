namespace TennisBookings.Configuration;

// Option frwarding implementation
// The interface is implemented by TennisBookings.Configuration.ContentConfiguration
public interface IContentConfiguration
{
	bool CheckForProfanity { get; } // intention is to implement a consumer abstraction, SET can be removed
}
