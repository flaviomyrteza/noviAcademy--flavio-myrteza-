namespace WorldRank;

public interface IPlayerRepository
{
    void AddPlayer(Player p);
    Player? FindPlayer(Guid playerId);
    bool DeletePlayer(Guid playerId);
    IEnumerable<Player> GetAll();

    /// <summary>Groups players by their current score.</summary>
    IEnumerable<IGrouping<int, Player>> GroupPlayersByScore();
}