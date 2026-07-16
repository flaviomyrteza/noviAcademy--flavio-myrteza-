namespace NoviCode.Services.Infrastructure
{
	public interface ICreatePlayerPersistence
	{
		Task Add(Player player);
	}

	public interface IUpdatePlayerScorePersistence
	{
		Task Update(Player player);
	}

	public interface IDeletePlayerPersistence
	{
		Task Delete(Guid playerId);
	}

	public interface IGetPlayerPersistence
	{
		Task<Player?> TryGet(Guid playerId);
		Task<Player> Get(Guid playerId);
	}

	public interface IGetAllPlayersPersistence
	{
		Task<IReadOnlyCollection<Player>> GetAll();
	}
}
