using MediatR;
using NoviCode.Dtos;

namespace NoviCode.Commands.Wallets;

// Command: deposit into a wallet by id. Returns the updated wallet, or null if it does not exist.
public record DepositCommand(Guid WalletId, decimal Amount) : IRequest<WalletDto?>;
