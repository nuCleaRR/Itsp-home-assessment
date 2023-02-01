using Microsoft.EntityFrameworkCore;

public class MovieService
{
    private readonly MovieContext _context;

    public MovieService(MovieContext context)
    {
        _context = context;
    }

    public async Task<List<Movie>> GetAllMovies()
    {
        return await _context.Movies
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Movie>> SearchAsync(MovieSearchCriteria criteria)
    {
        var filter = _context.Movies.AsQueryable();

        if (criteria.Genre.HasValue)
        {
            filter = filter.Where(x => x.Genre == criteria.Genre.Value);
        }

        if (!string.IsNullOrWhiteSpace(criteria.Name))
        {
            filter = filter.Where(x => x.Name.Contains(criteria.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        return await filter
            .AsNoTracking()
            .ToListAsync();
    }
}
