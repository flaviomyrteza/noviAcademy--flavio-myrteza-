namespace WorldRank;

public interface IWalletRepository
{
    void Add(Wallet wallet, Guid playerId);
    IEnumerable<Wallet> GetByPlayer(Guid playerId);
}