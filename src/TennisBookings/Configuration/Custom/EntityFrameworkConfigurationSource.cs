namespace TennisBookings.Configuration.Custom;

public class EntityFrameworkConfigurationSource : IConfigurationSource
{
	private readonly Action<DbContextOptionsBuilder> _optionsAction;

	public EntityFrameworkConfigurationSource(Action<DbContextOptionsBuilder> optionsAction)
	{
		_optionsAction = optionsAction;
	}

	// Call configuration provider created before passing the OptionsAction
	public IConfigurationProvider Build(IConfigurationBuilder builder) =>
		new EntityFrameworkConfigurationProvider(_optionsAction);
}
