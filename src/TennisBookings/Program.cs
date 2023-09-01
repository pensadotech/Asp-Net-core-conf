// This example uses Top Level statments 

#region Global Usings
// 'global using' is a notation to share 'using' statement across the solution
// avoiding to writing them in every class. This definition can be located at
// the entry point for the applicaiton or also can exist in a separate file
// GlobalUsings.cs at the root lebel
// Ref: https://blog.christian-schou.dk/why-and-how-to-use-global-usings-in-csharp/#:~:text=I%20always%20place%20mine%20at,as%20you%20would%20like%20to.

global using Microsoft.AspNetCore.Identity;

global using TennisBookings;
global using TennisBookings.Data;
global using TennisBookings.Domain;
global using TennisBookings.Extensions;
global using TennisBookings.Configuration;
global using TennisBookings.Caching;
global using TennisBookings.Shared.Weather;
global using TennisBookings.DependencyInjection;
global using TennisBookings.Services.Bookings;
global using TennisBookings.Services.Greetings;
global using TennisBookings.Services.Unavailability;
global using TennisBookings.Services.Bookings.Rules;
global using TennisBookings.Services.Notifications;
global using TennisBookings.Services.Time;
global using TennisBookings.Services.Staff;
global using TennisBookings.Services.Courts;
global using TennisBookings.Services.Security;
global using Microsoft.EntityFrameworkCore;
#endregion

using Microsoft.Data.Sqlite;
using TennisBookings.BackgroundService;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// DATABASE - it is using in memory database ( "SqliteConnection": "Filename=:memory:")
// The DB context is defined by TennisBookings.Data.TennisBookingsDbContext and is
// implementing Identity db context, adding security. It initialize DB information
// Down below it defines a background service to initalize users using IHostedService
using var connection = new SqliteConnection(builder.Configuration
	.GetConnectionString("SqliteConnection"));

await connection.OpenAsync();

// SERVICES

// Bidning
// builder.Services.Configure<HomePageConfiguration>(builder.Configuration.
//	GetSection("Features:HomePage"));

// To use Configurations Patterns (IOptionsSnapshot<T>)

// Example using IOption<T>  and data annotation attributes for configuration validation
//builder.Services.AddOptions<HomePageConfiguration>()
//	.Bind(builder.Configuration.GetSection("Features:HomePage"))
//	.ValidateDataAnnotations()  // This option indicate to enforce the attribute validations
//	.ValidateOnStart();         // evaluate if all config is available, otherwise fail to start

// Example using IOption<T> and configuration validate function
//builder.Services.AddOptions<HomePageConfiguration>()
//	.Bind(builder.Configuration.GetSection("Features:HomePage"))
//	.Validate(c =>
//	{
//		return !c.EnableWeatherForecast || !string.IsNullOrEmpty(c.ForecastSectionTitle);
//	}, "A section title must be provided when the homepage weather forecast is enabled.")
//	.ValidateOnStart();


// IOption for "Features:HomePage"
builder.Services.AddOptions<HomePageConfiguration>()
	.Bind(builder.Configuration.GetSection("Features:HomePage"))
	.ValidateOnStart();

// validation class for HomePageConfiguration
builder.Services.TryAddEnumerable(
	ServiceDescriptor.Singleton<IValidateOptions<HomePageConfiguration>,
		HomePageConfigurationValidation>());



// Conf binding
builder.Services.Configure<GreetingConfiguration>(builder.Configuration.
	         GetSection("Features:Greeting"));

// conf binding
//builder.Services.Configure<ExternalServicesConfiguration>(
//	ExternalServicesConfiguration.WeatherApi,
//	builder.Configuration.GetSection("ExternalServices:WeatherApi"));
//builder.Services.Configure<ExternalServicesConfiguration>(
//	ExternalServicesConfiguration.ProductsApi,
//	builder.Configuration.GetSection("ExternalServices:ProductsApi"));

// Name options 
builder.Services.AddOptions<ExternalServicesConfiguration>(
	ExternalServicesConfiguration.WeatherApi)
	.Bind(builder.Configuration.GetSection("ExternalServices:WeatherApi"))
	.ValidateOnStart();

builder.Services.AddOptions<ExternalServicesConfiguration>(
	ExternalServicesConfiguration.ProductsApi)
	.Bind(builder.Configuration.GetSection("ExternalServices:ProductsApi"))
	.ValidateOnStart();


// validation class for ExternalServicesConfiguration
builder.Services.TryAddEnumerable(
	ServiceDescriptor.Singleton<IValidateOptions<ExternalServicesConfiguration>,
		ExternalServicesConfigurationValidation>());


// Demostrates Binding and Option Pattern configuration
// Configuration Service that is an extension IServiceCollection.
// In this example the static class ConfigurationServiceCollectionExtensions, implement AddAppConfiguration
// and defines an STATIC class with an STATIC method that returns a IServiceCollection
// and has as parameters for the extension and IConfiguration : 
// 'this IServiceCollection services, IConfiguration config'
// This permist to load Configuration items into memory
builder.Services.AddAppConfiguration(builder.Configuration);

// Services extensions can be concatenated
// services extensions must state 'using Microsoft.Extensions.DependencyInjection.Extensions;'
// and defines an STATIC class with an STATIC method that returns a IServiceCollection
// and has as parameter for teh extension 'this IServiceCollection services'
// All services extensions are organized under teh folder 'Dependency injection'
builder.Services
	.AddBookingServices()
	.AddBookingRules()
	.AddCourtUnavailability()
	.AddMembershipServices()
	.AddStaffServices()
	.AddCourtServices()
	.AddWeatherForecasting(builder.Configuration)
	.AddProducts()
	.AddNotifications()
	.AddGreetings()
	.AddCaching()
	.AddTimeServices()
	.AddProfanityValidationService()
	.AddAuditing();

// Uses for MVC pattern but also include funcionality for APIs
builder.Services.AddControllersWithViews();

// uses Razor pages
builder.Services.AddRazorPages(options =>
{
	options.Conventions.AuthorizePage("/Bookings");
	options.Conventions.AuthorizePage("/BookCourt");
	options.Conventions.AuthorizePage("/FindAvailableCourts");
	options.Conventions.Add(new PageRouteTransformerConvention(new SlugifyParameterTransformer()));
});

// Add services to the container.
builder.Services.AddDbContext<TennisBookingsDbContext>(options => options.UseSqlite(connection));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<TennisBookingsUser, TennisBookingsRole>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddEntityFrameworkStores<TennisBookingsDbContext>()
	.AddDefaultUI()
	.AddDefaultTokenProviders();

// Background service to initalize users in the database
// AdminEmail = "admin@example.com";
// MemberEmail = "member@example.com";
// both uses as password = 'password'
builder.Services.AddHostedService<InitialiseDatabaseService>();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.AccessDeniedPath = "/identity/account/access-denied";
});

// add a way to display in the console the final settings loaded in memory
// WARNING: This will expose all configuration and there could be sesnitive data 
// that should not be exposed. Secrets are exposed in plan text.
//if (builder.Environment.IsDevelopment())
//{
//	var debugView = builder.Configuration.GetDebugView();
//	Console.WriteLine(debugView);
//}

// MIDDLEWAR COMPONENTS

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// MVC Routing, including API, with id as optional, similar to default routing
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

// RAZOR pages routing 
app.MapRazorPages();

// A DB initializer could be used here (e.g. DbInitializer.Seed(app);),
// but in this example the DB initialization occurs in the DBContext class and
// through a background service stated by the AddHostedService


app.Run();
