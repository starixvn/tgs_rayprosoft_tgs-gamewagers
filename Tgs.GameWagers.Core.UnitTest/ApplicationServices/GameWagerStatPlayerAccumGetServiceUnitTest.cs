using NSubstitute;
using Tgs.GameWagers.Core.ApplicationServices;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core.UnitTest.ApplicationServices;

public class GameWagerStatPlayerAccumGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeGameWagerStatCloseRepository = Substitute.For<IGameWagerStatCloseRepository>();
		var fakeGameWagerStatHourlyCloseRepository = Substitute.For<IGameWagerStatHourlyCloseRepository>();
		var fakeGameWagerStatDailyCloseRepository = Substitute.For<IGameWagerStatDailyCloseRepository>();

		var query = new GameWagerStatPlayerAccumQuery(
			"4ucmqplxb029");

		var currentTime = new DateTimeOffset(2024, 02, 23, 09, 30, 22, TimeSpan.FromHours(+08));
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		var minStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		var minEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 00, currentTime.Offset);
		var minStatPlayerAccumInfo = new GameWagerStatPlayerAccum(
			query.PlayerCode,
			1,
			100,
			100,
			0,
			0,
			100,
			-100);
		_ = fakeGameWagerStatCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			startOn: Arg.Is(minStartOn),
			endOn: Arg.Is(minEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(minStatPlayerAccumInfo);

		var hourlyStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, 00, 00, 00, currentTime.Offset);
		var hourlyEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		var hourlyStatPlayerAccumInfo = new GameWagerStatPlayerAccum(
			query.PlayerCode,
			10,
			2000,
			2000,
			1000,
			0,
			2000,
			-1000);
		_ = fakeGameWagerStatHourlyCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			startOn: Arg.Is(hourlyStartOn),
			endOn: Arg.Is(hourlyEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(hourlyStatPlayerAccumInfo);

		var dailyEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, 00, 00, 00, currentTime.Offset);
		var dailyStatPlayerAccumInfo = new GameWagerStatPlayerAccum(
			query.PlayerCode,
			100,
			10000,
			10000,
			20000,
			0,
			10000,
			10000);
		_ = fakeGameWagerStatDailyCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			endOn: Arg.Is(dailyEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(dailyStatPlayerAccumInfo);

		var sut = new GameWagerStatPlayerAccumGetService(
			fakeTimeProvider,
			fakeGameWagerStatCloseRepository,
			fakeGameWagerStatHourlyCloseRepository,
			fakeGameWagerStatDailyCloseRepository);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(minStatPlayerAccumInfo.Count + hourlyStatPlayerAccumInfo.Count + dailyStatPlayerAccumInfo.Count, actual.Count);
		Assert.Equal(minStatPlayerAccumInfo.Bet + hourlyStatPlayerAccumInfo.Bet + dailyStatPlayerAccumInfo.Bet, actual.Bet);
		Assert.Equal(minStatPlayerAccumInfo.ValidBet + hourlyStatPlayerAccumInfo.ValidBet + dailyStatPlayerAccumInfo.ValidBet, actual.ValidBet);
		Assert.Equal(minStatPlayerAccumInfo.Settle + hourlyStatPlayerAccumInfo.Settle + dailyStatPlayerAccumInfo.Settle, actual.Settle);
		Assert.Equal(minStatPlayerAccumInfo.Commission + hourlyStatPlayerAccumInfo.Commission + dailyStatPlayerAccumInfo.Commission, actual.Commission);
		Assert.Equal(minStatPlayerAccumInfo.Turnover + hourlyStatPlayerAccumInfo.Turnover + dailyStatPlayerAccumInfo.Turnover, actual.Turnover);
		Assert.Equal(minStatPlayerAccumInfo.WinLoss + hourlyStatPlayerAccumInfo.WinLoss + dailyStatPlayerAccumInfo.WinLoss, actual.WinLoss);
	}

	[Fact]
	public async Task Min_Is_Null()
	{
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeGameWagerStatCloseRepository = Substitute.For<IGameWagerStatCloseRepository>();
		var fakeGameWagerStatHourlyCloseRepository = Substitute.For<IGameWagerStatHourlyCloseRepository>();
		var fakeGameWagerStatDailyCloseRepository = Substitute.For<IGameWagerStatDailyCloseRepository>();

		var query = new GameWagerStatPlayerAccumQuery(
			"4ucmqplxb029");

		var currentTime = new DateTimeOffset(2024, 02, 23, 09, 30, 22, TimeSpan.FromHours(+08));
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		var minStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		var minEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 00, currentTime.Offset);
		_ = fakeGameWagerStatCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			startOn: Arg.Is(minStartOn),
			endOn: Arg.Is(minEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerStatPlayerAccum?)null);

		var hourlyStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, 00, 00, 00, currentTime.Offset);
		var hourlyEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		var hourlyStatPlayerAccumInfo = new GameWagerStatPlayerAccum(
			query.PlayerCode,
			10,
			2000,
			2000,
			1000,
			0,
			2000,
			-1000);
		_ = fakeGameWagerStatHourlyCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			startOn: Arg.Is(hourlyStartOn),
			endOn: Arg.Is(hourlyEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(hourlyStatPlayerAccumInfo);

		var dailyEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, 00, 00, 00, currentTime.Offset);
		var dailyStatPlayerAccumInfo = new GameWagerStatPlayerAccum(
			query.PlayerCode,
			100,
			10000,
			10000,
			20000,
			0,
			10000,
			10000);
		_ = fakeGameWagerStatDailyCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			endOn: Arg.Is(dailyEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(dailyStatPlayerAccumInfo);

		var sut = new GameWagerStatPlayerAccumGetService(
			fakeTimeProvider,
			fakeGameWagerStatCloseRepository,
			fakeGameWagerStatHourlyCloseRepository,
			fakeGameWagerStatDailyCloseRepository);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(hourlyStatPlayerAccumInfo.Count + dailyStatPlayerAccumInfo.Count, actual.Count);
		Assert.Equal(hourlyStatPlayerAccumInfo.Bet + dailyStatPlayerAccumInfo.Bet, actual.Bet);
		Assert.Equal(hourlyStatPlayerAccumInfo.ValidBet + dailyStatPlayerAccumInfo.ValidBet, actual.ValidBet);
		Assert.Equal(hourlyStatPlayerAccumInfo.Settle + dailyStatPlayerAccumInfo.Settle, actual.Settle);
		Assert.Equal(hourlyStatPlayerAccumInfo.Commission + dailyStatPlayerAccumInfo.Commission, actual.Commission);
		Assert.Equal(hourlyStatPlayerAccumInfo.Turnover + dailyStatPlayerAccumInfo.Turnover, actual.Turnover);
		Assert.Equal(hourlyStatPlayerAccumInfo.WinLoss + dailyStatPlayerAccumInfo.WinLoss, actual.WinLoss);
	}

	[Fact]
	public async Task Min_Is_Null_And_Hourly_Is_Null()
	{
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeGameWagerStatCloseRepository = Substitute.For<IGameWagerStatCloseRepository>();
		var fakeGameWagerStatHourlyCloseRepository = Substitute.For<IGameWagerStatHourlyCloseRepository>();
		var fakeGameWagerStatDailyCloseRepository = Substitute.For<IGameWagerStatDailyCloseRepository>();

		var query = new GameWagerStatPlayerAccumQuery(
			"4ucmqplxb029");

		var currentTime = new DateTimeOffset(2024, 02, 23, 09, 30, 22, TimeSpan.FromHours(+08));
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		var minStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		var minEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 00, currentTime.Offset);
		_ = fakeGameWagerStatCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			startOn: Arg.Is(minStartOn),
			endOn: Arg.Is(minEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerStatPlayerAccum?)null);

		var hourlyStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, 00, 00, 00, currentTime.Offset);
		var hourlyEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		_ = fakeGameWagerStatHourlyCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			startOn: Arg.Is(hourlyStartOn),
			endOn: Arg.Is(hourlyEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerStatPlayerAccum?)null);

		var dailyEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, 00, 00, 00, currentTime.Offset);
		var dailyStatPlayerAccumInfo = new GameWagerStatPlayerAccum(
			query.PlayerCode,
			100,
			10000,
			10000,
			20000,
			0,
			10000,
			10000);
		_ = fakeGameWagerStatDailyCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			endOn: Arg.Is(dailyEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(dailyStatPlayerAccumInfo);

		var sut = new GameWagerStatPlayerAccumGetService(
			fakeTimeProvider,
			fakeGameWagerStatCloseRepository,
			fakeGameWagerStatHourlyCloseRepository,
			fakeGameWagerStatDailyCloseRepository);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(dailyStatPlayerAccumInfo.Count, actual.Count);
		Assert.Equal(dailyStatPlayerAccumInfo.Bet, actual.Bet);
		Assert.Equal(dailyStatPlayerAccumInfo.ValidBet, actual.ValidBet);
		Assert.Equal(dailyStatPlayerAccumInfo.Settle, actual.Settle);
		Assert.Equal(dailyStatPlayerAccumInfo.Commission, actual.Commission);
		Assert.Equal(dailyStatPlayerAccumInfo.Turnover, actual.Turnover);
		Assert.Equal(dailyStatPlayerAccumInfo.WinLoss, actual.WinLoss);
	}

	[Fact]
	public async Task Min_Is_Null_And_Hourly_Is_Null_And_Daily_Is_Null()
	{
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeGameWagerStatCloseRepository = Substitute.For<IGameWagerStatCloseRepository>();
		var fakeGameWagerStatHourlyCloseRepository = Substitute.For<IGameWagerStatHourlyCloseRepository>();
		var fakeGameWagerStatDailyCloseRepository = Substitute.For<IGameWagerStatDailyCloseRepository>();

		var query = new GameWagerStatPlayerAccumQuery(
			"4ucmqplxb029");

		var currentTime = new DateTimeOffset(2024, 02, 23, 09, 30, 22, TimeSpan.FromHours(+08));
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		var minStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		var minEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 00, currentTime.Offset);
		_ = fakeGameWagerStatCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			startOn: Arg.Is(minStartOn),
			endOn: Arg.Is(minEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerStatPlayerAccum?)null);

		var hourlyStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, 00, 00, 00, currentTime.Offset);
		var hourlyEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		_ = fakeGameWagerStatHourlyCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			startOn: Arg.Is(hourlyStartOn),
			endOn: Arg.Is(hourlyEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerStatPlayerAccum?)null);

		var dailyEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, 00, 00, 00, currentTime.Offset);
		_ = fakeGameWagerStatDailyCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			endOn: Arg.Is(dailyEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerStatPlayerAccum?)null);

		var sut = new GameWagerStatPlayerAccumGetService(
			fakeTimeProvider,
			fakeGameWagerStatCloseRepository,
			fakeGameWagerStatHourlyCloseRepository,
			fakeGameWagerStatDailyCloseRepository);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(0, actual.Count);
		Assert.Equal(0.0m, actual.Bet);
		Assert.Equal(0.0m, actual.ValidBet);
		Assert.Equal(0.0m, actual.Settle);
		Assert.Equal(0.0m, actual.Commission);
		Assert.Equal(0.0m, actual.Turnover);
		Assert.Equal(0.0m, actual.WinLoss);
	}
}
