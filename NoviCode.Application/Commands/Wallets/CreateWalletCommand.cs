using MediatR;
using NoviCode.Dtos;

namespace NoviCode.Commands.Wallets;

// Command: create a new wallet for a player. Returns the created wallet.
public record CreateWalletCommand(Guid PlayerId, Currency Currency) : IRequest<WalletDto>;
