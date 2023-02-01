using System.Security.Claims;
using Itsp_home_assessment.Services.UserService;

public static class ApiEndpoints
{
    public static async Task<IResult> GetAllMovies(MovieService movieService)
    {
        return TypedResults.Ok(await movieService.GetAllMovies());
    }
    
    public static async Task<IResult> GetAllUsers(UserService userService)
    {
        return TypedResults.Ok(await userService.GetAllUsers());
    }
    
    public static async Task<IResult> GetUserById(Guid id, UserService userService) =>
        await userService.GetUser(id)
            is User user
            ? TypedResults.Ok(user)
            : TypedResults.NotFound();

    public static async Task<IResult> UpdateUser(User modifiedUser, ClaimsPrincipal userPrincipal, UserService userService) =>
        await userService.UpdateUser(modifiedUser, userPrincipal.Identity?.Name)
            is User user
            ? TypedResults.Ok(user)
            : TypedResults.Forbid();

    public static async Task<IResult> SearchMovieAsync([AsParameters] MovieSearchCriteria criteria, MovieService movieService) =>
        TypedResults.Ok(await movieService.SearchAsync(criteria));
}
