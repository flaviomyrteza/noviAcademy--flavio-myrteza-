using Microsoft.EntityFrameworkCore;
using NoviCode.Persistence.Context;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Queries.Players
{
	public class GetAllPlayersPersistence : IGetAllPlayersPersistence
	{
		private readonly AppDbContext _db;

		public GetAllPlayersPersistence(AppDbContext db)
		{
			_db = db;
		}

		public async Task<IReadOnlyCollection<Player>> GetAll()
		{
			return await _db.Players.ToArrayAsync();
		}
	}
}
