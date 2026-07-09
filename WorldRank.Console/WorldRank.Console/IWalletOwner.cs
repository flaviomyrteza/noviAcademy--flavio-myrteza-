namespace WorldRank;

/// <summary>
/// Anything that can hold wallets (one per currency) implements this.
/// This is the "link" between Player and Wallet required by the spec:
/// Player depends on this interface rather than Wallet depending on Player.
/// </summary>
public interface IWalletOwner
{
    Guid Id { get; }
    IReadOnlyCollection<Wallet> Wallets { get; }

    /// <summary>
    /// Attaches a wallet to the owner. Must enforce "one wallet per currency".
    /// Throws InvalidOperationException if a wallet for that currency already exists.
    /// </summary>
    void AttachWallet(Wallet wallet);
}