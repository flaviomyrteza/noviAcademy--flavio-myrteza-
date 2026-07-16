namespace NoviCode.Dtos;

// Read model returned by the player MediatR handlers (keeps the domain Player out of the API).
public record PlayerDto(Guid Id, string Name, int Score)
{
	public static PlayerDto From(Player player) => new(player.Id, player.Name, player.Score);
}
