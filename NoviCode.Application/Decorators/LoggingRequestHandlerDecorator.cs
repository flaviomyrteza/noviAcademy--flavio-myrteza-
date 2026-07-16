using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace NoviCode.Decorators;

// Decorator pattern: wraps EVERY MediatR request handler (queries and commands) to log its
// name, its parameters, and how long it took — without touching a single handler.
// Registered as an open-generic Autofac decorator over IRequestHandler<,> (ApplicationModule),
// so it wraps whatever concrete handler Autofac resolves for the request — same mechanism as
// the persistence caching decorators, just applied to handlers instead of persistence ports.
public class LoggingRequestHandlerDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IRequestHandler<TRequest, TResponse> _inner;
	private readonly ILogger<LoggingRequestHandlerDecorator<TRequest, TResponse>> _logger;

	public LoggingRequestHandlerDecorator(IRequestHandler<TRequest, TResponse> inner, ILogger<LoggingRequestHandlerDecorator<TRequest, TResponse>> logger)
	{
		_inner = inner;
		_logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;
		_logger.LogInformation("MediatR → {RequestName} {@Request}", requestName, request);

		var stopwatch = Stopwatch.StartNew();
		try
		{
			var response = await _inner.Handle(request, cancellationToken);
			stopwatch.Stop();
			_logger.LogInformation("MediatR ← {RequestName} handled in {ElapsedMs} ms", requestName, stopwatch.ElapsedMilliseconds);
			return response;
		}
		catch (Exception ex)
		{
			stopwatch.Stop();
			_logger.LogWarning("MediatR ✗ {RequestName} failed after {ElapsedMs} ms: {Error}",
				requestName, stopwatch.ElapsedMilliseconds, ex.Message);
			throw;
		}
	}
}
