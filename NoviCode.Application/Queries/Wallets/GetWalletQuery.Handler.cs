using MediatR;
using NoviCode.Dtos;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Queries.Wallets;

public class GetWalletQueryHandler : IRequestHandler<GetWalletQuery, WalletDto?>
{
	private readonly IGetWalletPersistence _persistence;

	public GetWalletQueryHandler(IGetWalletPersistence persistence)
	{
		_persistence = persistence;
	}

	public async Task<WalletDto?> Handle(GetWalletQuery request, CancellationToken cancellationToken)
	{
		var wallet = await _persistence.TryGet(request.WalletId);

		return wallet is null ? null : WalletDto.From(wallet);
	}
}
