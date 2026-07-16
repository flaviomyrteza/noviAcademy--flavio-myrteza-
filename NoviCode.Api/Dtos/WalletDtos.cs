using NoviCode;

namespace NoviCode.Api;

// Request DTOs — the shape the client sends. Kept separate from the domain entity.
public record CreateWalletRequest(Guid PlayerId, Currency Currency);

public record DepositRequest(decimal Amount);

// StrategyKey: "add" | "subtract" | "forcesubtract" — resolved to an IFundsStrategy by the factory.
public record AdjustBalanceRequest(decimal Amount, string StrategyKey);

// The response shape is the Application-layer WalletDto (returned by the MediatR handlers),
// so the controller no longer maps anything itself.
