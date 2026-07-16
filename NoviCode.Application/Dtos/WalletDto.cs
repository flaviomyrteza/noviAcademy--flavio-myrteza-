namespace NoviCode.Dtos;

// Read model returned by the MediatR query/command handlers. Lives in Application so the
// handlers never leak the domain Wallet entity to the API (and the API needs no mapping).
public record WalletDto(Guid Id, Guid PlayerId, string Currency, decimal Balance, bool IsBlocked)
{
	public static WalletDto From(Wallet wallet) =>
		new(wallet.Id, wallet.PlayerId, wallet.Currency.ToString(), wallet.Balance, wallet.IsBlocked);
}
