using MediatR;
using NoviCode.Dtos;

namespace NoviCode.Commands.Wallets;

// Command: unblock a wallet by id. Returns the updated wallet, or null if it does not exist.
public record UnblockWalletCommand(Guid WalletId) : IRequest<WalletDto?>;
