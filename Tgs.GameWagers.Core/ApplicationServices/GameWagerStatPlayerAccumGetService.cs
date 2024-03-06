using System.Text.Json;
using Microsoft.Extensions.Logging;
using Tgs.GameWagers.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameWagers.Core.ApplicationServices;

internal class GameWagerStatPlayerAccumGetService(
	ILogger<GameWagerStatPlayerAccumGetService> logger,
	TimeProvider timeProvider,
	IGameWagerPlayerAccumRepository gameWagerPlayerAccumRepository,
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

		var currentTimeUTC8 = currentTime.ToOffset(TimeSpan.FromHours(+8));

		logger.LogInformation("LogOn:{logOn} CurrentTimeUTC8:{currentTimeUTC8}",
			DateTime.Now.ToString("o"),
			currentTimeUTC8);

		var count = 0;
		var bet = 0.0m;
		var validBet = 0.0m;
		var settle = 0.0m;
		var commission = 0.0m;
		var turnover = 0.0m;
		var winLoss = 0.0m;

		var gameWagerPlayerAccum = await gameWagerPlayerAccumRepository.GetNewestAsync(
			playerCode: query.PlayerCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var accumOn = DateTimeOffset.UnixEpoch;

		if (gameWagerPlayerAccum is not null)
		{
			accumOn = gameWagerPlayerAccum.AccumOn;
			count += gameWagerPlayerAccum.Count;
			bet += gameWagerPlayerAccum.Bet;
			validBet += gameWagerPlayerAccum.ValidBet;
			settle += gameWagerPlayerAccum.Settle;
			commission += gameWagerPlayerAccum.Commission;
			turnover += gameWagerPlayerAccum.Turnover;
			winLoss += gameWagerPlayerAccum.WinLoss;
		}

		var minStartOn = new DateTimeOffset(currentTimeUTC8.Year, currentTimeUTC8.Month, currentTimeUTC8.Day, currentTimeUTC8.Hour, 00, 00, currentTimeUTC8.Offset);
		var minEndOn = new DateTimeOffset(currentTimeUTC8.Year, currentTimeUTC8.Month, currentTimeUTC8.Day, currentTimeUTC8.Hour, currentTimeUTC8.Minute, 00, currentTimeUTC8.Offset);

		logger.LogInformation("LogOn:{logOn} MinStartOn:{minStartOn} MinEndOn:{minEndOn}",
			DateTime.Now.ToString("o"),
			minStartOn,
			minEndOn);

		var gameWagerStatPlayerAccumMin = await statCloseRepository.GetPlayerAccumAsync(
			playerCode: query.PlayerCode,
			startOn: minStartOn,
			endOn: minEndOn,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		logger.LogInformation("LogOn:{logOn} MinStatPlayerAccumInfo:{minStatPlayerAccumInfo}",
			DateTime.Now.ToString("o"),
			JsonSerializer.Serialize(gameWagerStatPlayerAccumMin));

		if (gameWagerStatPlayerAccumMin is not null)
		{
			count += gameWagerStatPlayerAccumMin.Count;
			bet += gameWagerStatPlayerAccumMin.Bet;
			validBet += gameWagerStatPlayerAccumMin.ValidBet;
			settle += gameWagerStatPlayerAccumMin.Settle;
			commission += gameWagerStatPlayerAccumMin.Commission;
			turnover += gameWagerStatPlayerAccumMin.Turnover;
			winLoss += gameWagerStatPlayerAccumMin.WinLoss;
		}

		var dateOn = DateOnly.FromDateTime(currentTimeUTC8.Date);
		var endTimeOn = TimeOnly.FromTimeSpan(TimeSpan.FromHours(currentTimeUTC8.Hour));

		logger.LogInformation("LogOn:{logOn} DateOn:{dateOn} EndTimeOn:{endTimeOn}",
			DateTime.Now.ToString("o"),
			dateOn,
			endTimeOn);

		var gameWagerStatPlayerAccumHourly = await statHourlyCloseRepository.GetPlayerAccumAsync(
			playerCode: query.PlayerCode,
			dateOn: dateOn,
			endTimeOn: endTimeOn,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		logger.LogInformation("LogOn:{logOn} HourlyStatPlayerAccumInfo:{hourlyStatPlayerAccumInfo}",
			DateTime.Now.ToString("o"),
			JsonSerializer.Serialize(gameWagerStatPlayerAccumHourly));

		if (gameWagerStatPlayerAccumHourly is not null)
		{
			count += gameWagerStatPlayerAccumHourly.Count;
			bet += gameWagerStatPlayerAccumHourly.Bet;
			validBet += gameWagerStatPlayerAccumHourly.ValidBet;
			settle += gameWagerStatPlayerAccumHourly.Settle;
			commission += gameWagerStatPlayerAccumHourly.Commission;
			turnover += gameWagerStatPlayerAccumHourly.Turnover;
			winLoss += gameWagerStatPlayerAccumHourly.WinLoss;
		}

		var startDayOn = DateOnly.FromDateTime(accumOn.ToOffset(TimeSpan.FromHours(+8)).Date);
		var endDayOn = DateOnly.FromDateTime(currentTimeUTC8.Date);
		logger.LogInformation("LogOn:{logOn} StartDayOn:{startDayOn} EndDayOn:{endDayOn}",
			DateTime.Now.ToString("o"),
			startDayOn,
			endDayOn);

		var gameWagerStatPlayerAccumDaily = await statDailyCloseRepository.GetPlayerAccumAsync(
			playerCode: query.PlayerCode,
			startDayOn: startDayOn,
			endDayOn: endDayOn,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		logger.LogInformation("LogOn:{logOn} DailyStatPlayerAccumInfo:{dailyStatPlayerAccumInfo}",
			DateTime.Now.ToString("o"),
			JsonSerializer.Serialize(gameWagerStatPlayerAccumDaily));

		if (gameWagerStatPlayerAccumDaily is not null)
		{
			count += gameWagerStatPlayerAccumDaily.Count;
			bet += gameWagerStatPlayerAccumDaily.Bet;
			validBet += gameWagerStatPlayerAccumDaily.ValidBet;
			settle += gameWagerStatPlayerAccumDaily.Settle;
			commission += gameWagerStatPlayerAccumDaily.Commission;
			turnover += gameWagerStatPlayerAccumDaily.Turnover;
			winLoss += gameWagerStatPlayerAccumDaily.WinLoss;
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
