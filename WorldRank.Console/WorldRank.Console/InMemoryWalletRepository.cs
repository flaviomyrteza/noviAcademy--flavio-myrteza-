namespace WorldRank;

public class InMemoryWalletRepository : IWalletRepository
{
    // playerId -> wallets
    private readonly Dictionary<Guid, List<Wallet>> _walletsByPlayer = new();

    public void Add(Wallet wallet, Guid playerId)
    {
        ArgumentNullException.ThrowIfNull(wallet);

        if (!_walletsByPlayer.TryGetValue(playerId, out var wallets))
        {
            wallets = new List<Wallet>();
            _walletsByPlayer[playerId] = wallets;
        }

        if (wallets.Any(w => w.Currency == wallet.Currency))
            throw new InvalidOperationException(
                $"Player {playerId} already has a {wallet.Currency} wallet.");

        wallets.Add(wallet);
    }

    public IEnumerable<Wallet> GetByPlayer(Guid playerId)
    {
        return _walletsByPlayer.TryGetValue(playerId, out var wallets)
            ? wallets.AsReadOnly()
            : Enumerable.Empty<Wallet>();
    }
}