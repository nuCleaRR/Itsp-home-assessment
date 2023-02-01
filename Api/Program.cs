using Itsp_home_assessment.Api.Infrastructure;
using Itsp_home_assessment.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// use Serilog as default log provider
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Logging.ClearProviders();
builder.Host.UseSerilog((_, _, conf) =>
{
    conf.WriteTo.Console();

    if (!builder.Environment.IsDevelopment())
    {
        // TelemetryConfiguration.Active is deprecated.it's possible to overcome, just need an effort and a lot of testing :) 
        // conf.WriteTo.ApplicationInsights(TelemetryConfiguration.Active, TelemetryConverter.Events);
    }
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer"
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }

    });
});

builder.Services.AddProblemDetails();

builder.Services.AddAuthentication()
    .AddJwtBearer();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("admin_policy", policy =>
        policy
            .RequireRole("admin"))
    .AddPolicy("user_policy", policy =>
        policy
            .RequireRole("user", "admin"));

var loggerConfiguration = new LoggerConfiguration()
    .WriteTo.Console();
    
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<MovieContext>(opt => opt.UseInMemoryDatabase("MovieDatabase"));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}
else
{
    // connection to the real database should be there
}

builder.Services.AddTransient<MovieService>();
builder.Services.AddTransient<UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.SeedTestData();
}

app.UseExceptionHandler();

app.MapGet("/movies/get-all", ApiEndpoints.GetAllMovies)
    .RequireAuthorization("admin_policy")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized);

app.MapGet("/users/get-all", ApiEndpoints.GetAllUsers)
    .RequireAuthorization("admin_policy")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized);

app.MapGet("/users/{id:guid}", ApiEndpoints.GetUserById)
    .RequireAuthorization("user_policy")
    .Produces<User>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status401Unauthorized);

app.MapPut("/users/", ApiEndpoints.UpdateUser)
    .RequireAuthorization("user_policy")
    .Produces<User>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status403Forbidden)
    .Produces(StatusCodes.Status401Unauthorized);

app.MapGet("/movies", ApiEndpoints.SearchMovieAsync)
    .RequireAuthorization("user_policy")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized)
    .WithOpenApi(operation =>
    {
        operation.Description = "Searches movies by given criteria";

        var genreParameter = operation.Parameters[1];
        genreParameter.Description = $"Possible values are " + string.Join(", ", Enum.GetNames<Genre>());

        return operation;
    });

app.Run();

public partial class Program
{
    // to be able to use it in test project
}