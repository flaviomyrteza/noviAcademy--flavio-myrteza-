using MediatR;
using NoviCode.Dtos;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Commands.Wallets;

public class DepositCommandHandler : IRequestHandler<DepositCommand, WalletDto?>
{
	private readonly IGetWalletPersistence _getWalletPersistence;
	private readonly IUpdateWalletPersistence _updateWalletPersistence;

	public DepositCommandHandler(IGetWalletPersistence getWalletPersistence, IUpdateWalletPersistence updateWalletPersistence)
	{
		_getWalletPersistence = getWalletPersistence;
		_updateWalletPersistence = updateWalletPersistence;
	}

	public async Task<WalletDto?> Handle(DepositCommand request, CancellationToken cancellationToken)
	{
		var wallet = await _getWalletPersistence.TryGet(request.WalletId);
		if (wallet is null)
			return null;

		wallet.Deposit(request.Amount); // may throw WalletBlockedException / InvalidAmountException

		await _updateWalletPersistence.Update(wallet);

		return WalletDto.From(wallet);
	}
}
