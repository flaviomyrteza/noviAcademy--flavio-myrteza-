using MediatR;
using NoviCode.Dtos;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Commands.Wallets;

// Resolves the correct IFundsStrategy from the factory (no if/switch) and applies it.
public class AdjustBalanceCommandHandler : IRequestHandler<AdjustBalanceCommand, WalletDto?>
{
	private readonly IGetWalletPersistence _getWalletPersistence;
	private readonly IUpdateWalletPersistence _updateWalletPersistence;
	private readonly FundsStrategyFactory _strategyFactory;

	public AdjustBalanceCommandHandler(
		IGetWalletPersistence getWalletPersistence,
		IUpdateWalletPersistence updateWalletPersistence,
		FundsStrategyFactory strategyFactory)
	{
		_getWalletPersistence = getWalletPersistence;
		_updateWalletPersistence = updateWalletPersistence;
		_strategyFactory = strategyFactory;
	}

	public async Task<WalletDto?> Handle(AdjustBalanceCommand request, CancellationToken cancellationToken)
	{
		var strategy = _strategyFactory.Resolve(request.StrategyKey); // throws ArgumentException on unknown key

		var wallet = await _getWalletPersistence.TryGet(request.WalletId);
		if (wallet is null)
			return null;

		strategy.Execute(wallet, request.Amount); // may throw WalletException (blocked / insufficient / invalid)

		await _updateWalletPersistence.Update(wallet);

		return WalletDto.From(wallet);
	}
}
