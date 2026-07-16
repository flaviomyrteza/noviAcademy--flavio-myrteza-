using MediatR;
using NoviCode.Dtos;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Commands.Wallets;

public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, WalletDto>
{
	private readonly ICreateWalletPersistence _persistence;

	public CreateWalletCommandHandler(ICreateWalletPersistence persistence)
	{
		_persistence = persistence;
	}

	public async Task<WalletDto> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
	{
		var wallet = Wallet.CreateNew(Guid.NewGuid(), request.PlayerId, request.Currency);

		await _persistence.Add(wallet);

		return WalletDto.From(wallet);
	}
}
