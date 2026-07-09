namespace WorldRank;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly Dictionary<Guid, Player> _players = new();

    public void AddPlayer(Player p)
    {
        ArgumentNullException.ThrowIfNull(p);

        if (_players.ContainsKey(p.Id))
            throw new InvalidOperationException($"A player with Id {p.Id} already exists.");

        _players[p.Id] = p;
    }

    public Player? FindPlayer(Guid playerId)
    {
        return _players.TryGetValue(playerId, out var player) ? player : null;
    }

    public bool DeletePlayer(Guid playerId)
    {
        return _players.Remove(playerId);
    }

    public IEnumerable<Player> GetAll() => _players.Values.AsEnumerable();

    public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
    {
        return _players.Values.GroupBy(p => p.Score);
    }
}