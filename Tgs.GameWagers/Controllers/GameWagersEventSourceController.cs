using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Controllers;
[Route("game-wagers-event-source")]
[ApiController]
public class GameWagersEventSourceController : ControllerBase
{
	[HttpGet("all/id/{gameProviderId:guid}")]
	public async IAsyncEnumerable<GameWagersEventSourceInfo> QueryAllAsync(
		[FromServices] IGameWagersEventSourceRepository repository,
		Guid gameProviderId,
		[FromQuery] Guid id,
		[FromQuery] int limit= 100)
	{
		var models = await repository.QueryAllAsync(
			gameProviderId: gameProviderId,
			id: id,
			limit: limit,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagersEventSourceModel>();

		foreach (var model in models)
		{
			yield return new GameWagersEventSourceInfo(
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
				WagerState: model.WagerState,
				WagerType: model.WagerType,
				FreeGameState: model.FreeGameState,
				GambleState: model.GambleState,
				CashoutState: model.CashoutState,
				ResettleState: model.ResettleState,
				ExternalRoundId: model.ExternalRoundId,
				ExternalWagerId: model.ExternalWagerId,
				ExternalCreatedOn: model.ExternalCreatedOn,
				ExternalClosedOn: model.ExternalClosedOn,
				ExternalPayoffOn: model.ExternalPayoffOn,
				ExternalResettleOn: model.ExternalResettleOn,
				CurrencyRatio: model.CurrencyRatio,
				Bet: model.Bet,
				ValidBet: model.ValidBet,
				Settle: model.Settle,
				Commission: model.Commission,
				Turnover: model.Turnover,
				WinLoss: model.WinLoss,
				AdditionalData: JsonNode.Parse(model.AdditionalData) ?? new JsonObject { });
		}
	}

	[HttpGet("settlements/id/{gameProviderId:guid}")]
	public async IAsyncEnumerable<GameWagersEventSourceInfo> QuerySettlementsAsync(
		[FromServices] IGameWagersEventSourceRepository repository,
		Guid gameProviderId,
		[FromQuery] Guid id,
		[FromQuery] int limit= 100)
	{
		var models = await repository.QuerySettlementsAsync(
			gameProviderId: gameProviderId,
			id: id,
			limit: limit,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagersEventSourceModel>();

		foreach (var model in models)
		{
			yield return new GameWagersEventSourceInfo(
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
				WagerState: model.WagerState,
				WagerType: model.WagerType,
				FreeGameState: model.FreeGameState,
				GambleState: model.GambleState,
				CashoutState: model.CashoutState,
				ResettleState: model.ResettleState,
				ExternalRoundId: model.ExternalRoundId,
				ExternalWagerId: model.ExternalWagerId,
				ExternalCreatedOn: model.ExternalCreatedOn,
				ExternalClosedOn: model.ExternalClosedOn,
				ExternalPayoffOn: model.ExternalPayoffOn,
				ExternalResettleOn: model.ExternalResettleOn,
				CurrencyRatio: model.CurrencyRatio,
				Bet: model.Bet,
				ValidBet: model.ValidBet,
				Settle: model.Settle,
				Commission: model.Commission,
				Turnover: model.Turnover,
				WinLoss: model.WinLoss,
				AdditionalData: JsonNode.Parse(model.AdditionalData) ?? new JsonObject { });
		}
	}

	[HttpGet("settlements/timepoint/{gameProviderId:guid}")]
	public async IAsyncEnumerable<GameWagersEventSourceInfo> QuerySettlementsAsync(
		[FromServices] IGameWagersEventSourceRepository repository,
		Guid gameProviderId,
		[FromQuery] DateTimeOffset timepoint,
		[FromQuery] int limit= 100)
	{
		var models = await repository.QuerySettlementsAsync(
			gameProviderId: gameProviderId,
			timepoint: timepoint,
			limit: limit,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagersEventSourceModel>();

		foreach (var model in models)
		{
			yield return new GameWagersEventSourceInfo(
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
				WagerState: model.WagerState,
				WagerType: model.WagerType,
				FreeGameState: model.FreeGameState,
				GambleState: model.GambleState,
				CashoutState: model.CashoutState,
				ResettleState: model.ResettleState,
				ExternalRoundId: model.ExternalRoundId,
				ExternalWagerId: model.ExternalWagerId,
				ExternalCreatedOn: model.ExternalCreatedOn,
				ExternalClosedOn: model.ExternalClosedOn,
				ExternalPayoffOn: model.ExternalPayoffOn,
				ExternalResettleOn: model.ExternalResettleOn,
				CurrencyRatio: model.CurrencyRatio,
				Bet: model.Bet,
				ValidBet: model.ValidBet,
				Settle: model.Settle,
				Commission: model.Commission,
				Turnover: model.Turnover,
				WinLoss: model.WinLoss,
				AdditionalData: JsonNode.Parse(model.AdditionalData) ?? new JsonObject { });
		}
	}

	[HttpGet("settlements/time-interval/{gameProviderId:guid}")]
	public async IAsyncEnumerable<GameWagersEventSourceInfo> QuerySettlementsAsync(
		[FromServices] IGameWagersEventSourceRepository repository,
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
			?? Enumerable.Empty<GameWagersEventSourceModel>();

		foreach (var model in models)
		{
			yield return new GameWagersEventSourceInfo(
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
				WagerState: model.WagerState,
				WagerType: model.WagerType,
				FreeGameState: model.FreeGameState,
				GambleState: model.GambleState,
				CashoutState: model.CashoutState,
				ResettleState: model.ResettleState,
				ExternalRoundId: model.ExternalRoundId,
				ExternalWagerId: model.ExternalWagerId,
				ExternalCreatedOn: model.ExternalCreatedOn,
				ExternalClosedOn: model.ExternalClosedOn,
				ExternalPayoffOn: model.ExternalPayoffOn,
				ExternalResettleOn: model.ExternalResettleOn,
				CurrencyRatio: model.CurrencyRatio,
				Bet: model.Bet,
				ValidBet: model.ValidBet,
				Settle: model.Settle,
				Commission: model.Commission,
				Turnover: model.Turnover,
				WinLoss: model.WinLoss,
				AdditionalData: JsonNode.Parse(model.AdditionalData) ?? new JsonObject { });
		}
	}
}
