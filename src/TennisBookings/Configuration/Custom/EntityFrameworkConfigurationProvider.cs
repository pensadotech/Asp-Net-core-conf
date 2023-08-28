namespace TennisBookings.Configuration.Custom;

// Used for the custom configuration provder from teh database
// THis will be called from a source class

public class EntityFrameworkConfigurationProvider : ConfigurationProvider
{
	// Properties
	public Action<DbContextOptionsBuilder> OptionsAction { get; }

	// Constructor
	public EntityFrameworkConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
	{
		OptionsAction = optionsAction;
	}

	// Methods
	public override void Load()
	{
		// create an instance of the DB context buildr
		var builder = new DbContextOptionsBuilder<TennisBookingsDbContext>();

		// Call s Option action, passing int teh build
		OptionsAction(builder);

		// Populate the Data property on the base class

		// Create DB context passing teh builder
		using var dbContext = new TennisBookingsDbContext(builder.Options);

		// because a in-memory DB is used, make sure teh DB is created
		dbContext.Database.EnsureCreated();

		// retreive teh data and add it to a Dictionary that holds teh kesy and values
		Data = dbContext.ConfigurationEntries.Any()
			? dbContext.ConfigurationEntries.ToDictionary(entry => entry.Key,
				entry => entry.Value, StringComparer.OrdinalIgnoreCase)
			: new Dictionary<string, string>();
	}
}
