using MediatR;
using NoviCode.Dtos;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Commands.Wallets;

public class BlockWalletCommandHandler : IRequestHandler<BlockWalletCommand, WalletDto?>
{
	private readonly IGetWalletPersistence _getWalletPersistence;
	private readonly IUpdateWalletPersistence _updateWalletPersistence;

	public BlockWalletCommandHandler(IGetWalletPersistence getWalletPersistence, IUpdateWalletPersistence updateWalletPersistence)
	{
		_getWalletPersistence = getWalletPersistence;
		_updateWalletPersistence = updateWalletPersistence;
	}

	public async Task<WalletDto?> Handle(BlockWalletCommand request, CancellationToken cancellationToken)
	{
		var wallet = await _getWalletPersistence.TryGet(request.WalletId);
		if (wallet is null)
			return null;

		wallet.Block();

		await _updateWalletPersistence.Update(wallet);

		return WalletDto.From(wallet);
	}
}
