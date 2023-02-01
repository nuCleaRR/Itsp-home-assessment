using Microsoft.EntityFrameworkCore;

namespace Itsp_home_assessment.Services.UserService;

public sealed class UserService
{
    private readonly MovieContext _movieContext;

    public UserService(MovieContext movieContext)
    {
        _movieContext = movieContext;
    }

    public async Task<List<User>> GetAllUsers() =>
        await _movieContext.Users
            .Include(x => x.Movies)
            .AsNoTracking()
            .ToListAsync();
    
    public async Task<User> GetUser(Guid id) =>
        await _movieContext.Users
            .Include(x => x.Movies)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User> UpdateUser(User modifiedUser, string? identityName)
    {
        var user = await GetUser(modifiedUser.Id);
        if (user == null)
        {
            return null;
        }
        
        var userName = identityName;
        
        if (string.IsNullOrWhiteSpace(identityName) || userName != user.Email)
        {
            return null;
        }
        
        _movieContext.Entry(modifiedUser).State = EntityState.Modified;
        await _movieContext.SaveChangesAsync();
 
        return modifiedUser;
    }
}