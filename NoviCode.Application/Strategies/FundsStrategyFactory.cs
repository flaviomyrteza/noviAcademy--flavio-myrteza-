using Autofac.Features.Indexed;

namespace NoviCode;

// Factory pattern (Day 7): turns a string key into the right IFundsStrategy, so callers
// (the MediatR handlers) never contain an if/switch over strategy types.
//
// IIndex<string, IFundsStrategy> is Autofac's lookup for KEYED registrations: the module
// registers each strategy under a key ("add", "subtract", "forcesubtract") and Autofac
// hands this factory a dictionary-like view over them.
public class FundsStrategyFactory
{
	private readonly IIndex<string, IFundsStrategy> _strategies;

	public FundsStrategyFactory(IIndex<string, IFundsStrategy> strategies) => _strategies = strategies;

	public IFundsStrategy Resolve(string key)
	{
		if (string.IsNullOrWhiteSpace(key) || !_strategies.TryGetValue(key.ToLowerInvariant(), out var strategy))
			throw new ArgumentException($"No funds strategy is registered for key '{key}'.", nameof(key));

		return strategy;
	}
}
