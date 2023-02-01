public record Movie
{
    public required Guid Id { get; init; } = Guid.NewGuid();

    public required string Name { get; init; } = string.Empty;

    public required  Genre Genre { get; init; } = Genre.NotSet;

    public required  int Rating { get; init; } = -1;
}
