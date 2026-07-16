using Autofac;
using NoviCode.Caching;
using NoviCode.Persistence.Commands.Players;
using NoviCode.Persistence.Commands.Wallets;
using NoviCode.Persistence.Queries.Players;
using NoviCode.Persistence.Queries.Wallets;
using NoviCode.Services.Infrastructure;

namespace NoviCode
{
	public class InfrastructureModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			// Players

			builder.RegisterType<CreatePlayerPersistence>()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			builder.RegisterType<GetPlayerPersistence>()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			builder.RegisterType<GetAllPlayersPersistence>()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			builder.RegisterType<UpdatePlayerScorePersistence>()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			builder.RegisterType<DeletePlayerPersistence>()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			builder.RegisterDecorator<CreatePlayerPersistenceChachingDecorator, ICreatePlayerPersistence>();
			builder.RegisterDecorator<GetPlayerPersistenceCachingDecorator, IGetPlayerPersistence>();
			builder.RegisterDecorator<UpdatePlayerScorePersistenceCachingDecorator, IUpdatePlayerScorePersistence>();
			builder.RegisterDecorator<DeletePlayerPersistenceCachingDecorator, IDeletePlayerPersistence>();

			builder.RegisterType<PlayersCache>()
				.AsImplementedInterfaces()
				.SingleInstance();

			// Wallets
			builder.RegisterType<CreateWalletPersistence>()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			builder.RegisterType<GetWalletPersistence>()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			builder.RegisterType<UpdateWalletPersistence>()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			builder.RegisterDecorator<CreateWalletPersistenceCachingDecorator, ICreateWalletPersistence>();
			builder.RegisterDecorator<GetWalletPersistenceCachingDecorator, IGetWalletPersistence>();
			builder.RegisterDecorator<UpdateWalletPersistenceCachingDecorator, IUpdateWalletPersistence>();

			builder.RegisterType<WalletsCache>()
				.AsImplementedInterfaces()
				.SingleInstance();
		}
	}
}
