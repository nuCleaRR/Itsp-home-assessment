using Microsoft.EntityFrameworkCore;

public sealed class MovieContext : DbContext
{
    public MovieContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Movie> Movies => Set<Movie>();
    
    public DbSet<User> Users => Set<User>();
}
