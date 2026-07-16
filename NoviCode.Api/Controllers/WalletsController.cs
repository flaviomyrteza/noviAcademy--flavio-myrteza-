using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoviCode.Commands.Wallets;
using NoviCode.Queries.Wallets;

namespace NoviCode.Api;

[ApiController]
[Route("wallets")]
public class WalletsController : ControllerBase
{
	// Day 7: the controller depends ONLY on the mediator. No service is called directly —
	// every operation is a query or command dispatched through the MediatR pipeline.
	private readonly ISender _mediator;

	public WalletsController(ISender mediator) => _mediator = mediator;

	// POST /wallets — create a wallet, return 201 Created with a Location header.
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateWalletRequest request, CancellationToken cancellationToken)
	{
		var wallet = await _mediator.Send(new CreateWalletCommand(request.PlayerId, request.Currency), cancellationToken);
		return CreatedAtAction(nameof(GetById), new { id = wallet.Id }, wallet);
	}

	// GET /wallets/{id} — goes through the GetWalletQuery handler (verified in the logs).
	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
	{
		var wallet = await _mediator.Send(new GetWalletQuery(id), cancellationToken);
		return wallet is null ? NotFound() : Ok(wallet);
	}

	// POST /wallets/{id}/deposit — deposit funds.
	[HttpPost("{id:guid}/deposit")]
	public async Task<IActionResult> Deposit(Guid id, [FromBody] DepositRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var wallet = await _mediator.Send(new DepositCommand(id, request.Amount), cancellationToken);
			return wallet is null ? NotFound() : Ok(wallet);
		}
		catch (WalletException ex)
		{
			return BadRequest(new { error = ex.Message });
		}
	}

	// POST /wallets/{id}/adjustbalance — apply a strategy chosen at runtime by StrategyKey.
	[HttpPost("{id:guid}/adjustbalance")]
	public async Task<IActionResult> AdjustBalance(Guid id, [FromBody] AdjustBalanceRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var wallet = await _mediator.Send(new AdjustBalanceCommand(id, request.Amount, request.StrategyKey), cancellationToken);
			return wallet is null ? NotFound() : Ok(wallet);
		}
		catch (ArgumentException ex)
		{
			// Unknown strategy key → 400.
			return BadRequest(new { error = ex.Message });
		}
		catch (WalletException ex)
		{
			// Business-rule violation (blocked wallet, insufficient funds, non-positive amount) → 400.
			return BadRequest(new { error = ex.Message });
		}
	}

	// POST /wallets/{id}/block — block a wallet.
	[HttpPost("{id:guid}/block")]
	public async Task<IActionResult> Block(Guid id, CancellationToken cancellationToken)
	{
		var wallet = await _mediator.Send(new BlockWalletCommand(id), cancellationToken);
		return wallet is null ? NotFound() : Ok(wallet);
	}

	// POST /wallets/{id}/unblock — unblock a wallet.
	[HttpPost("{id:guid}/unblock")]
	public async Task<IActionResult> Unblock(Guid id, CancellationToken cancellationToken)
	{
		var wallet = await _mediator.Send(new UnblockWalletCommand(id), cancellationToken);
		return wallet is null ? NotFound() : Ok(wallet);
	}
}
