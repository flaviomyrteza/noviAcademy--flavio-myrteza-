using MediatR;
using NoviCode.Dtos;

namespace NoviCode.Queries.Wallets;

// Query: fetch a wallet by id. Returns null if it does not exist.
public record GetWalletQuery(Guid WalletId) : IRequest<WalletDto?>;
