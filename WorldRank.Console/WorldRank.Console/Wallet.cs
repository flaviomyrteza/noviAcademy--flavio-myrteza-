namespace WorldRank;

/// <summary>
/// Balance can never go negative and no external code can set it directly
/// (no public setter) — the only ways to change it are Deposit/Withdraw,
/// which themselves enforce the invariant.
/// </summary>
public class Wallet
{
    public Guid Id { get; } = Guid.NewGuid();
    public Currency Currency { get; }
    public decimal Balance { get; private set; }
    public bool IsBlocked { get; private set; }

    public Wallet(Currency currency, decimal openingBalance = 0m)
    {
        if (openingBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(openingBalance), "Opening balance cannot be negative.");

        Currency = currency;
        Balance = openingBalance;
    }

    public void Deposit(decimal amount)
    {
        EnsureNotBlocked();
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Deposit amount must be positive.");

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        EnsureNotBlocked();
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Withdrawal amount must be positive.");
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds: withdrawal would make balance negative.");

        Balance -= amount;
    }

    public void Block() => IsBlocked = true;
    public void Unblock() => IsBlocked = false;

    private void EnsureNotBlocked()
    {
        if (IsBlocked)
            throw new InvalidOperationException("Wallet is blocked; no transactions are allowed.");
    }

    public override string ToString() =>
        $"{Currency}: {Balance:0.00}{(IsBlocked ? " [BLOCKED]" : "")}";
}