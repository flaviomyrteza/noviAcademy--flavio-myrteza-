using Autofac;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using NoviCode.Decorators;

namespace NoviCode;

// Autofac module: groups every Application-layer registration in one place (Day 7).
// Program.cs replaces the built-in container with Autofac and loads this module.
public class ApplicationModule : Autofac.Module
{
	protected override void Load(ContainerBuilder builder)
	{
		var applicationAssembly = typeof(ApplicationModule).Assembly;

		// --- MediatR ---
		// The mediator itself resolves handlers/behaviours through the IServiceProvider at send time.
		var configuration = MediatRConfigurationBuilder
			.Create(ThisAssembly)
			.WithAllOpenGenericHandlerTypesRegistered()
			.Build();

		builder.RegisterMediatR(configuration);

		// Decorator (open generic) — LoggingRequestHandlerDecorator wraps every request
		// handler Autofac resolves, the same way the persistence caching decorators wrap
		// their inner persistence implementation.
		builder.RegisterGenericDecorator(typeof(LoggingRequestHandlerDecorator<,>), typeof(IRequestHandler<,>));

		// --- Strategies: KEYED registrations (Factory pattern) ---
		// The factory looks these up by key via IIndex<string, IFundsStrategy> — no if/switch.
		builder.RegisterType<AddFundsStrategy>().Keyed<IFundsStrategy>("add").InstancePerLifetimeScope();
		builder.RegisterType<SubtractFundsStrategy>().Keyed<IFundsStrategy>("subtract").InstancePerLifetimeScope();
		builder.RegisterType<ForceSubtractFundsStrategy>().Keyed<IFundsStrategy>("forcesubtract").InstancePerLifetimeScope();

		builder.RegisterType<FundsStrategyFactory>().AsSelf().InstancePerLifetimeScope();
	}
}
