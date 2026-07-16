using NoviCode.Persistence.Context;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Commands.Players
{
	public class CreatePlayerPersistence : ICreatePlayerPersistence
	{
		private readonly AppDbContext _db;

		public CreatePlayerPersistence(AppDbContext db)
		{
			_db = db;
		}

		public async Task Add(Player player)
		{
			_db.Players.Add(player);

			await _db.SaveChangesAsync();
		}
	}
}
