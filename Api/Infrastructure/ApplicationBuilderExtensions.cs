using Bogus;

namespace Itsp_home_assessment.Api.Infrastructure;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder SeedTestData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<MovieContext>();
        context.Users.AddRange(GenerateTestData());
        context.SaveChanges();

        return app;
    }

    private static List<User> GenerateTestData()
    {
        var fakerMovie = new Faker<Movie>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Genre, f => f.PickRandomWithout<Genre>(Genre.NotSet))
            .RuleFor(x => x.Name, f => f.Company.CompanyName() + " super movie")
            .RuleFor(x => x.Rating, f => f.Random.Int(0, 10))
            .UseSeed(8675309);

        var fakerUser = new Faker<User>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Name, f => f.Name.FirstName())
            .RuleFor(x => x.Movies, f => fakerMovie.GenerateBetween(5, 25))
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .UseSeed(8675309);

        return fakerUser.Generate(10);
    }
}