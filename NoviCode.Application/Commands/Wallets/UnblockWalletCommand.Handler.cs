using MediatR;
using NoviCode.Dtos;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Commands.Wallets;

public class UnblockWalletCommandHandler : IRequestHandler<UnblockWalletCommand, WalletDto?>
{
	private readonly IGetWalletPersistence _getWalletPersistence;
	private readonly IUpdateWalletPersistence _updateWalletPersistence;

	public UnblockWalletCommandHandler(IGetWalletPersistence getWalletPersistence, IUpdateWalletPersistence updateWalletPersistence)
	{
		_getWalletPersistence = getWalletPersistence;
		_updateWalletPersistence = updateWalletPersistence;
	}

	public async Task<WalletDto?> Handle(UnblockWalletCommand request, CancellationToken cancellationToken)
	{
		var wallet = await _getWalletPersistence.TryGet(request.WalletId);
		if (wallet is null)
			return null;

		wallet.Unblock();

		await _updateWalletPersistence.Update(wallet);

		return WalletDto.From(wallet);
	}
}
