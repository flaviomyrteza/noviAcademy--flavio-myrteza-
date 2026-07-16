using Microsoft.EntityFrameworkCore;
using NoviCode.Persistence.Context;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Commands.Players
{
	public class DeletePlayerPersistence : IDeletePlayerPersistence
	{
		private readonly AppDbContext _db;

		public DeletePlayerPersistence(AppDbContext db)
		{
			_db = db;
		}

		public async Task Delete(Guid playerId)
		{
			await _db.Players
				.Where(x => x.Id == playerId)
				.ExecuteDeleteAsync();
		}
	}
}
