using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoviCode.Commands.Players;
using NoviCode.Queries.Players;

namespace NoviCode.Api;

[ApiController]
[Route("players")]
public class PlayersController : ControllerBase
{
	// Like WalletsController, this depends only on the mediator — every action is a query/command.
	private readonly ISender _mediator;

	public PlayersController(ISender mediator) => _mediator = mediator;

	// POST /players — create a player.
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreatePlayerRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var player = await _mediator.Send(new CreatePlayerCommand(request.Name, request.Score), cancellationToken);

			return CreatedAtAction(nameof(GetById), new { Id = player.Id }, player);
		}
		catch (ArgumentException ex)
		{
			// Empty name / negative score → 400.
			return BadRequest(new { error = ex.Message });
		}
	}

	// GET /players/{id} — 200 or 404 (goes through the GetPlayerQuery handler).
	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
	{
		var player = await _mediator.Send(new GetPlayerQuery(id), cancellationToken);

		return player is null ? NotFound() : Ok(player);
	}

	// GET /players — list all players.
	[HttpGet]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		var players = await _mediator.Send(new GetAllPlayersQuery(), cancellationToken);
		return Ok(players);
	}

	// PUT /players/{id}/score — update a player's score.
	[HttpPut("{id:guid}/score")]
	public async Task<IActionResult> UpdatePlayerScore(Guid id, UpdatePlayerScore request, CancellationToken cancellationToken)
	{
		await _mediator.Send(new UpdatePlayerScoreCommand(id, request.Score), cancellationToken);

		return Ok();
	}

	// DELETE /players/{id} — delete a player.
	[HttpDelete]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		await _mediator.Send(new DeletePlayerCommand(id), cancellationToken);

		return Ok();
	}
}
