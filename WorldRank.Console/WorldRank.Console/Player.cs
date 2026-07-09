namespace WorldRank;

public class Player : IPlayer , IWalletOwner
{
	public Guid Id { get; }
	public string Name { get; }
	public int Score { get; private set; }

	public Player(string name)
	{
		if (string.IsNullOrEmpty(name))
			throw new ArgumentException("Name cannot be null or empty.", nameof(name));

		Id = Guid.NewGuid();
		Name = name;
	}

	public void UpdateScore(int newScore)
	{
		if (newScore < 0)
			throw new ArgumentOutOfRangeException(nameof(newScore), "Score cannot be negative.");

		Score = newScore;
	}

	public override string ToString() =>
			$"[{Id}] {Name} - Score: {Score}";

    private readonly List<Wallet> _wallets = new();

    public IReadOnlyCollection<Wallet> Wallets => _wallets.AsReadOnly();

    public void AttachWallet(Wallet wallet)
    {
        ArgumentNullException.ThrowIfNull(wallet);

        if (_wallets.Any(w => w.Currency == wallet.Currency))
            throw new InvalidOperationException(
                $"Player '{Name}' already has a wallet in {wallet.Currency}.");

        _wallets.Add(wallet);
    }

}

