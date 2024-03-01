using System.Text.Json;
using Microsoft.Extensions.Logging;
using Tgs.GameWagers.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameWagers.Core.ApplicationServices;

internal class GameWagerStatPlayerAccumGetService(
	ILogger<GameWagerStatPlayerAccumGetService> logger,
	TimeProvider timeProvider,
	IGameWagerStatCloseRepository statCloseRepository,
	IGameWagerStatHourlyCloseRepository statHourlyCloseRepository,
	IGameWagerStatDailyCloseRepository statDailyCloseRepository) : IGetService<GameWagerStatPlayerAccumQuery, GameWagerStatPlayerAccumInfo>
{
	public async ValueTask<GameWagerStatPlayerAccumInfo> GetAsync(GameWagerStatPlayerAccumQuery query, CancellationToken cancellationToken = default)
	{
		var currentTime = timeProvider.GetUtcNow();

		logger.LogInformation("LogOn:{logOn} CurrentTime:{currentTime}",
			DateTime.Now.ToString("o"),
			currentTime);

		var count = 0;
		var bet = 0.0m;
		var validBet = 0.0m;
		var settle = 0.0m;
		var commission = 0.0m;
		var turnover = 0.0m;
		var winLoss = 0.0m;

		var minStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		var minEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 00, currentTime.Offset);

		logger.LogInformation("LogOn:{logOn} MinStartOn:{minStartOn} MinEndOn:{minEndOn}",
			DateTime.Now.ToString("o"),
			minStartOn,
			minEndOn);

		var minStatPlayerAccumInfo = await statCloseRepository.GetPlayerAccumAsync(
			playerCode: query.PlayerCode,
			startOn: minStartOn,
			endOn: minEndOn,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		logger.LogInformation("LogOn:{logOn} MinStatPlayerAccumInfo:{minStatPlayerAccumInfo}",
			DateTime.Now.ToString("o"),
			JsonSerializer.Serialize(minStatPlayerAccumInfo));

		if (minStatPlayerAccumInfo is not null)
		{
			count += minStatPlayerAccumInfo?.Count ?? 0;
			bet += minStatPlayerAccumInfo?.Bet ?? 0;
			validBet += minStatPlayerAccumInfo?.ValidBet ?? 0;
			settle += minStatPlayerAccumInfo?.Settle ?? 0;
			commission += minStatPlayerAccumInfo?.Commission ?? 0;
			turnover += minStatPlayerAccumInfo?.Turnover ?? 0;
			winLoss += minStatPlayerAccumInfo?.WinLoss ?? 0;
		}

		var hourlyStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, 00, 00, 00, currentTime.Offset);
		var hourlyEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);

		logger.LogInformation("LogOn:{logOn} HourlyStartOn:{hourlyStartOn} HourlyEndOn:{hourlyEndOn}",
			DateTime.Now.ToString("o"),
			hourlyStartOn,
			hourlyEndOn);

		var hourlyStatPlayerAccumInfo = await statHourlyCloseRepository.GetPlayerAccumAsync(
			playerCode: query.PlayerCode,
			startOn: hourlyStartOn,
			endOn: hourlyEndOn,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		logger.LogInformation("LogOn:{logOn} HourlyStatPlayerAccumInfo:{hourlyStatPlayerAccumInfo}",
			DateTime.Now.ToString("o"),
			JsonSerializer.Serialize(hourlyStatPlayerAccumInfo));

		if (hourlyStatPlayerAccumInfo is not null)
		{
			count += hourlyStatPlayerAccumInfo?.Count ?? 0;
			bet += hourlyStatPlayerAccumInfo?.Bet ?? 0;
			validBet += hourlyStatPlayerAccumInfo?.ValidBet ?? 0;
			settle += hourlyStatPlayerAccumInfo?.Settle ?? 0;
			commission += hourlyStatPlayerAccumInfo?.Commission ?? 0;
			turnover += hourlyStatPlayerAccumInfo?.Turnover ?? 0;
			winLoss += hourlyStatPlayerAccumInfo?.WinLoss ?? 0;
		}

		var dailyStatPlayerAccumInfo = await statDailyCloseRepository.GetPlayerAccumAsync(
			playerCode: query.PlayerCode,
			endOn: currentTime,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		logger.LogInformation("LogOn:{logOn} DailyStatPlayerAccumInfo:{dailyStatPlayerAccumInfo}",
			DateTime.Now.ToString("o"),
			JsonSerializer.Serialize(dailyStatPlayerAccumInfo));

		if (dailyStatPlayerAccumInfo is not null)
		{
			count += dailyStatPlayerAccumInfo?.Count ?? 0;
			bet += dailyStatPlayerAccumInfo?.Bet ?? 0;
			validBet += dailyStatPlayerAccumInfo?.ValidBet ?? 0;
			settle += dailyStatPlayerAccumInfo?.Settle ?? 0;
			commission += dailyStatPlayerAccumInfo?.Commission ?? 0;
			turnover += dailyStatPlayerAccumInfo?.Turnover ?? 0;
			winLoss += dailyStatPlayerAccumInfo?.WinLoss ?? 0;
		}

		logger.LogInformation("LogOn:{logOn} GameWagerStatPlayerAccumInfo:{gameWagerStatPlayerAccumInfo}",
			DateTime.Now.ToString("o"),
			JsonSerializer.Serialize(new GameWagerStatPlayerAccumInfo(
				PlayerCode: query.PlayerCode,
				Count: count,
				Bet: bet,
				ValidBet: validBet,
				Settle: settle,
				Commission: commission,
				Turnover: turnover,
				WinLoss: winLoss)));

		return new GameWagerStatPlayerAccumInfo(
			 PlayerCode: query.PlayerCode,
			 Count: count,
			 Bet: bet,
			 ValidBet: validBet,
			 Settle: settle,
			 Commission: commission,
			 Turnover: turnover,
			 WinLoss: winLoss);
	}
}
