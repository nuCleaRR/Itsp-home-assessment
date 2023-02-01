public record User
{
    public required Guid Id { get; init; } = Guid.NewGuid();

    public required string Name { get; init; }
    
    public required string Email { get; init; }

    public required List<Movie>? Movies { get; init; }
}
