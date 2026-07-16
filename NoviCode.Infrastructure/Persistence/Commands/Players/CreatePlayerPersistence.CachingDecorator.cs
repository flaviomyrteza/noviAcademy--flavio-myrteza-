using NoviCode.Caching;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Commands.Players
{
	public class CreatePlayerPersistenceChachingDecorator : ICreatePlayerPersistence
	{
		private readonly IPlayersCache _cache;
		private readonly ICreatePlayerPersistence _inner;

		public CreatePlayerPersistenceChachingDecorator(IPlayersCache cache, ICreatePlayerPersistence inner)
		{
			_cache = cache;
			_inner = inner;
		}

		public async Task Add(Player player)
		{
			await _inner.Add(player);

			_cache.AddOrUpdatePlayer(player);
		}
	}
}
