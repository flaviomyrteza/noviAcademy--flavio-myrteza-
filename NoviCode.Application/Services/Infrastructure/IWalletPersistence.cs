namespace NoviCode.Services.Infrastructure
{
	public interface ICreateWalletPersistence
	{
		Task Add(Wallet wallet);
	}

	public interface IUpdateWalletPersistence
	{
		Task Update(Wallet wallet);
	}

	public interface IGetWalletPersistence
	{
		Task<Wallet?> TryGet(Guid walletId);
	}
}
