using MediatR;
using NoviCode.Dtos;

namespace NoviCode.Commands.Wallets;

// Command: adjust a wallet's balance using a strategy chosen at runtime by StrategyKey
// ("add" | "subtract" | "forcesubtract"). Returns the updated wallet, or null if not found.
//
// NOTE: the Day 7 doc suggests IRequest<Unit>; we return WalletDto? instead so the endpoint
// can answer 404 vs 200 and echo the new balance back to the caller.
public record AdjustBalanceCommand(Guid WalletId, decimal Amount, string StrategyKey) : IRequest<WalletDto?>;
