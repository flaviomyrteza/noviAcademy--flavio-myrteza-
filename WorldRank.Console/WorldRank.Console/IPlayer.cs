namespace WorldRank;

public interface IPlayer
{
    Guid Id { get; }
    string Name { get; }
    int Score { get; }
}