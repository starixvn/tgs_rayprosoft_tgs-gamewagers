using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Controllers;

[Route("game-bets-event-source")]
[ApiController]
public class GameBetsEventSourceController : ControllerBase
{
	/// <summary>
	/// Queries all asynchronous.
	/// </summary>
	/// <param name="repository">The repository.</param>
	/// <param name="gameProviderId">The game provider identifier.</param>
	/// <param name="id">The identifier.</param>
	/// <param name="limit">The limit.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Get
	///			gameProviderId->a0f49ce0-2c1d-4f50-9fc5-58b610a30bf5
	///			id->00000000-0000-0000-0000-000000000000
	///			limit->100
	/// </remarks>
	[HttpGet("all/id/{gameProviderId:guid}")]
	public async IAsyncEnumerable<GameBetsEventSourceInfo> QueryAllAsync(
		[FromServices] IGameBetEventSourceRepository repository,
		Guid gameProviderId,
		[FromQuery] Guid id,
		[FromQuery] int limit = 100)
	{
		var models = await repository.QueryAllAsync(
			gameProviderId: gameProviderId,
			id: id,
			limit: limit,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameBetEventSourceModel>();

		foreach (var model in models)
		{
			yield return new GameBetsEventSourceInfo(
				Id: model.Id,
				CreatedOn: model.CreatedOn,
				GameProviderId: model.GameProviderId,
				GameProviderConfigurationId: model.GameProviderConfigurationId,
				GameId: model.GameId,
				ResellerId: model.ResellerId,
				LicenseeId: model.LicenseeId,
				BrandId: model.BrandId,
				PlayerCode: model.PlayerCode,
				CurrencyCode: model.CurrencyCode,
				WalletId: model.WalletId,
				ExternalRoundId: model.ExternalRoundId,
				ExternalBetId: model.ExternalBetId,
				ExternalCreatedOn: model.ExternalCreatedOn,
				ExternalClosedOn: model.ExternalClosedOn,
				ExternalPayoffOn: model.ExternalPayoffOn,
				ExternalResettleOn: model.ExternalResettleOn,
				BetState: model.BetState,
				BetType: model.BetType,
				FreeGameState: model.FreeGameState,
				GambleState: model.GambleState,
				CashoutState: model.CashoutState,
				ResettleState: model.ResettleState,
				JackpotState: model.JackpotState,
				CurrencyRatio: model.CurrencyRatio,
				Bet: model.Bet,
				ValidBet: model.ValidBet,
				Settle: model.Settle,
				Commission: model.Commission,
				Turnover: model.Turnover,
				WinLoss: model.WinLoss,
				Jackpot: model.Jackpot,
				AdditionalData: JsonNode.Parse(model.AdditionalData) ?? new JsonObject { });
		}
	}

	/// <summary>
	/// Queries all asynchronous.
	/// </summary>
	/// <param name="repository">The repository.</param>
	/// <param name="gameProviderId">The game provider identifier.</param>
	/// <param name="fromOn">From on.</param>
	/// <param name="toOn">To on.</param>
	/// <param name="page">The page.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Get
	///			gameProviderId->a0f49ce0-2c1d-4f50-9fc5-58b610a30bf5
	///			fromOn->2024-03-12T16:00:00Z
	///			toOn->2024-03-13T16:00:00Z
	///			page->1
	///			pageSize->100
	/// </remarks>
	[HttpGet("all/time-interval/{gameProviderId:guid}")]
	public async IAsyncEnumerable<GameBetsEventSourceInfo> QueryAllAsync(
		[FromServices] IGameBetEventSourceRepository repository,
		Guid gameProviderId,
		[FromQuery] DateTimeOffset fromOn,
		[FromQuery] DateTimeOffset toOn,
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 100)
	{
		var models = await repository.QueryAllAsync(
			gameProviderId: gameProviderId,
			fromOn: fromOn,
			toOn: toOn,
			offset: page - 1,
			limit: pageSize,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameBetEventSourceModel>();

		foreach (var model in models)
		{
			yield return new GameBetsEventSourceInfo(
				Id: model.Id,
				CreatedOn: model.CreatedOn,
				GameProviderId: model.GameProviderId,
				GameProviderConfigurationId: model.GameProviderConfigurationId,
				GameId: model.GameId,
				ResellerId: model.ResellerId,
				LicenseeId: model.LicenseeId,
				BrandId: model.BrandId,
				PlayerCode: model.PlayerCode,
				CurrencyCode: model.CurrencyCode,
				WalletId: model.WalletId,
				ExternalRoundId: model.ExternalRoundId,
				ExternalBetId: model.ExternalBetId,
				ExternalCreatedOn: model.ExternalCreatedOn,
				ExternalClosedOn: model.ExternalClosedOn,
				ExternalPayoffOn: model.ExternalPayoffOn,
				ExternalResettleOn: model.ExternalResettleOn,
				BetState: model.BetState,
				BetType: model.BetType,
				FreeGameState: model.FreeGameState,
				GambleState: model.GambleState,
				CashoutState: model.CashoutState,
				ResettleState: model.ResettleState,
				JackpotState: model.JackpotState,
				CurrencyRatio: model.CurrencyRatio,
				Bet: model.Bet,
				ValidBet: model.ValidBet,
				Settle: model.Settle,
				Commission: model.Commission,
				Turnover: model.Turnover,
				WinLoss: model.WinLoss,
				Jackpot: model.Jackpot,
				AdditionalData: JsonNode.Parse(model.AdditionalData) ?? new JsonObject { });
		}
	}

	/// <summary>
	/// Queries the settlements asynchronous.
	/// </summary>
	/// <param name="repository">The repository.</param>
	/// <param name="gameProviderId">The game provider identifier.</param>
	/// <param name="id">The identifier.</param>
	/// <param name="limit">The limit.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Get
	///			gameProviderId->a0f49ce0-2c1d-4f50-9fc5-58b610a30bf5
	///			id->00000000-0000-0000-0000-000000000000
	///			limit->100
	/// </remarks>
	[HttpGet("settlements/id/{gameProviderId:guid}")]
	public async IAsyncEnumerable<GameBetsEventSourceInfo> QuerySettlementsAsync(
		[FromServices] IGameBetEventSourceRepository repository,
		Guid gameProviderId,
		[FromQuery] Guid id,
		[FromQuery] int limit = 100)
	{
		var models = await repository.QuerySettlementsAsync(
			gameProviderId: gameProviderId,
			id: id,
			limit: limit,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameBetEventSourceModel>();

		foreach (var model in models)
		{
			yield return new GameBetsEventSourceInfo(
				Id: model.Id,
				CreatedOn: model.CreatedOn,
				GameProviderId: model.GameProviderId,
				GameProviderConfigurationId: model.GameProviderConfigurationId,
				GameId: model.GameId,
				ResellerId: model.ResellerId,
				LicenseeId: model.LicenseeId,
				BrandId: model.BrandId,
				PlayerCode: model.PlayerCode,
				CurrencyCode: model.CurrencyCode,
				WalletId: model.WalletId,
				ExternalRoundId: model.ExternalRoundId,
				ExternalBetId: model.ExternalBetId,
				ExternalCreatedOn: model.ExternalCreatedOn,
				ExternalClosedOn: model.ExternalClosedOn,
				ExternalPayoffOn: model.ExternalPayoffOn,
				ExternalResettleOn: model.ExternalResettleOn,
				BetState: model.BetState,
				BetType: model.BetType,
				FreeGameState: model.FreeGameState,
				GambleState: model.GambleState,
				CashoutState: model.CashoutState,
				ResettleState: model.ResettleState,
				JackpotState: model.JackpotState,
				CurrencyRatio: model.CurrencyRatio,
				Bet: model.Bet,
				ValidBet: model.ValidBet,
				Settle: model.Settle,
				Commission: model.Commission,
				Turnover: model.Turnover,
				WinLoss: model.WinLoss,
				Jackpot: model.Jackpot,
				AdditionalData: JsonNode.Parse(model.AdditionalData) ?? new JsonObject { });
		}
	}

	/// <summary>
	/// Queries the settlements asynchronous.
	/// </summary>
	/// <param name="repository">The repository.</param>
	/// <param name="gameProviderId">The game provider identifier.</param>
	/// <param name="timepoint">The timepoint.</param>
	/// <param name="limit">The limit.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Get
	///			gameProviderId->a0f49ce0-2c1d-4f50-9fc5-58b610a30bf5
	///			timepoint->1970-01-01T00:00:00Z
	///			limit->100
	/// </remarks>
	[HttpGet("settlements/timepoint/{gameProviderId:guid}")]
	public async IAsyncEnumerable<GameBetsEventSourceInfo> QuerySettlementsAsync(
		[FromServices] IGameBetEventSourceRepository repository,
		Guid gameProviderId,
		[FromQuery] DateTimeOffset timepoint,
		[FromQuery] int limit = 100)
	{
		var models = await repository.QuerySettlementsAsync(
			gameProviderId: gameProviderId,
			timepoint: timepoint,
			limit: limit,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameBetEventSourceModel>();

		foreach (var model in models)
		{
			yield return new GameBetsEventSourceInfo(
				Id: model.Id,
				CreatedOn: model.CreatedOn,
				GameProviderId: model.GameProviderId,
				GameProviderConfigurationId: model.GameProviderConfigurationId,
				GameId: model.GameId,
				ResellerId: model.ResellerId,
				LicenseeId: model.LicenseeId,
				BrandId: model.BrandId,
				PlayerCode: model.PlayerCode,
				CurrencyCode: model.CurrencyCode,
				WalletId: model.WalletId,
				ExternalRoundId: model.ExternalRoundId,
				ExternalBetId: model.ExternalBetId,
				ExternalCreatedOn: model.ExternalCreatedOn,
				ExternalClosedOn: model.ExternalClosedOn,
				ExternalPayoffOn: model.ExternalPayoffOn,
				ExternalResettleOn: model.ExternalResettleOn,
				BetState: model.BetState,
				BetType: model.BetType,
				FreeGameState: model.FreeGameState,
				GambleState: model.GambleState,
				CashoutState: model.CashoutState,
				ResettleState: model.ResettleState,
				JackpotState: model.JackpotState,
				CurrencyRatio: model.CurrencyRatio,
				Bet: model.Bet,
				ValidBet: model.ValidBet,
				Settle: model.Settle,
				Commission: model.Commission,
				Turnover: model.Turnover,
				WinLoss: model.WinLoss,
				Jackpot: model.Jackpot,
				AdditionalData: JsonNode.Parse(model.AdditionalData) ?? new JsonObject { });
		}
	}

	/// <summary>
	/// Queries the settlements asynchronous.
	/// </summary>
	/// <param name="repository">The repository.</param>
	/// <param name="gameProviderId">The game provider identifier.</param>
	/// <param name="fromOn">From on.</param>
	/// <param name="toOn">To on.</param>
	/// <param name="page">The page.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Get
	///			gameProviderId->a0f49ce0-2c1d-4f50-9fc5-58b610a30bf5
	///			fromOn->2024-03-12T16:00:00Z
	///			toOn->2024-03-13T16:00:00Z
	///			page->1
	///			pageSize->100
	/// </remarks>
	[HttpGet("settlements/time-interval/{gameProviderId:guid}")]
	public async IAsyncEnumerable<GameBetsEventSourceInfo> QuerySettlementsAsync(
		[FromServices] IGameBetEventSourceRepository repository,
		Guid gameProviderId,
		[FromQuery] DateTimeOffset fromOn,
		[FromQuery] DateTimeOffset toOn,
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 100)
	{
		var models = await repository.QuerySettlementsAsync(
			gameProviderId: gameProviderId,
			fromOn: fromOn,
			toOn: toOn,
			offset: page - 1,
			limit: pageSize,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameBetEventSourceModel>();

		foreach (var model in models)
		{
			yield return new GameBetsEventSourceInfo(
				Id: model.Id,
				CreatedOn: model.CreatedOn,
				GameProviderId: model.GameProviderId,
				GameProviderConfigurationId: model.GameProviderConfigurationId,
				GameId: model.GameId,
				ResellerId: model.ResellerId,
				LicenseeId: model.LicenseeId,
				BrandId: model.BrandId,
				PlayerCode: model.PlayerCode,
				CurrencyCode: model.CurrencyCode,
				WalletId: model.WalletId,
				ExternalRoundId: model.ExternalRoundId,
				ExternalBetId: model.ExternalBetId,
				ExternalCreatedOn: model.ExternalCreatedOn,
				ExternalClosedOn: model.ExternalClosedOn,
				ExternalPayoffOn: model.ExternalPayoffOn,
				ExternalResettleOn: model.ExternalResettleOn,
				BetState: model.BetState,
				BetType: model.BetType,
				FreeGameState: model.FreeGameState,
				GambleState: model.GambleState,
				CashoutState: model.CashoutState,
				ResettleState: model.ResettleState,
				JackpotState: model.JackpotState,
				CurrencyRatio: model.CurrencyRatio,
				Bet: model.Bet,
				ValidBet: model.ValidBet,
				Settle: model.Settle,
				Commission: model.Commission,
				Turnover: model.Turnover,
				WinLoss: model.WinLoss,
				Jackpot: model.Jackpot,
				AdditionalData: JsonNode.Parse(model.AdditionalData) ?? new JsonObject { });
		}
	}

	/// <summary>
	/// Queries all asynchronous.
	/// </summary>
	/// <param name="repository">The repository.</param>
	/// <param name="id">The identifier.</param>
	/// <param name="createdOn">The created on.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Get
	///			id->018e6420-7b86-0000-0001-07e822d8e6e5
	///			createdOn->2024-03-22T03:07:14Z
	/// </remarks>
	[HttpGet("{id:guid}")]
	public async ValueTask<GameBetsEventSourceInfo?> GetAsync(
		[FromServices] IGameBetEventSourceRepository repository,
		Guid id,
		[FromQuery] DateTimeOffset createdOn)
	{
		var model = await repository.GetAsync(
			id: id,
			createdOn: createdOn,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return model is null
			? null
			: new GameBetsEventSourceInfo(
				Id: model.Id,
				CreatedOn: model.CreatedOn,
				GameProviderId: model.GameProviderId,
				GameProviderConfigurationId: model.GameProviderConfigurationId,
				GameId: model.GameId,
				ResellerId: model.ResellerId,
				LicenseeId: model.LicenseeId,
				BrandId: model.BrandId,
				PlayerCode: model.PlayerCode,
				CurrencyCode: model.CurrencyCode,
				WalletId: model.WalletId,
				ExternalRoundId: model.ExternalRoundId,
				ExternalBetId: model.ExternalBetId,
				ExternalCreatedOn: model.ExternalCreatedOn,
				ExternalClosedOn: model.ExternalClosedOn,
				ExternalPayoffOn: model.ExternalPayoffOn,
				ExternalResettleOn: model.ExternalResettleOn,
				BetState: model.BetState,
				BetType: model.BetType,
				FreeGameState: model.FreeGameState,
				GambleState: model.GambleState,
				CashoutState: model.CashoutState,
				ResettleState: model.ResettleState,
				JackpotState: model.JackpotState,
				CurrencyRatio: model.CurrencyRatio,
				Bet: model.Bet,
				ValidBet: model.ValidBet,
				Settle: model.Settle,
				Commission: model.Commission,
				Turnover: model.Turnover,
				WinLoss: model.WinLoss,
				Jackpot: model.Jackpot,
				AdditionalData: JsonNode.Parse(model.AdditionalData) ?? new JsonObject { });
	}
}
