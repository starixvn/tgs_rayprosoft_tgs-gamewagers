using System.Threading;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.GameWagers.Core.ApplicationServices;
using Tgs.GameWagers.Core.Models;

namespace Tgs.GameWagers.Core.UnitTest.ApplicationServices;

public class GameWagerStatPlayerAccumGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeLog = NullLogger<GameWagerStatPlayerAccumGetService>.Instance;
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeGameWagerPlayerAccumRepository = Substitute.For<IGameWagerPlayerAccumRepository>();
		var fakeGameWagerStatCloseRepository = Substitute.For<IGameWagerStatCloseRepository>();
		var fakeGameWagerStatHourlyCloseRepository = Substitute.For<IGameWagerStatHourlyCloseRepository>();
		var fakeGameWagerStatDailyCloseRepository = Substitute.For<IGameWagerStatDailyCloseRepository>();

		var query = new GameWagerStatPlayerAccumQuery(
			"4ucmqplxb029");

		var currentTime = new DateTimeOffset(2024, 02, 23, 09, 30, 22, TimeSpan.FromHours(+08));
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		var gameWagerPlayerAccum = new GameWagerPlayerAccumModel(
			PlayerCode: "4ucmqplxb029",
			AccumOn: new DateTimeOffset(2023, 10, 01, 00, 00, 00, TimeSpan.FromHours(+08)),
			Count: 1771,
			Bet: 44055.900000m,
			ValidBet: 42403.600000m,
			Settle: 0.00m,
			Commission: 0.00m,
			Turnover: 42376.900000m,
			WinLoss: -38458.265001m);
		_ = fakeGameWagerPlayerAccumRepository.GetNewestAsync(
			playerCode: query.PlayerCode,
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameWagerPlayerAccum);

		var minStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		var minEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 00, currentTime.Offset);
		var minStatPlayerAccum = new GameWagerStatPlayerAccumModel(
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
			.Returns(minStatPlayerAccum);

		var dateOn = DateOnly.FromDateTime(currentTime.Date);
		var endTimeOn = TimeOnly.FromTimeSpan(TimeSpan.FromHours(currentTime.Hour));
		var gameWagerStatPlayerAccumHourly = new GameWagerStatPlayerAccumModel(
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
			dateOn: Arg.Is(dateOn),
			endTimeOn: Arg.Is(endTimeOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameWagerStatPlayerAccumHourly);

		var gameWagerStatPlayerAccumDaily = new GameWagerStatPlayerAccumModel(
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
			startDayOn: Arg.Is(DateOnly.FromDateTime(gameWagerPlayerAccum.AccumOn.ToOffset(TimeSpan.FromHours(+8)).Date)),
			endDayOn: Arg.Is(DateOnly.FromDateTime(currentTime.Date)),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameWagerStatPlayerAccumDaily);

		var sut = new GameWagerStatPlayerAccumGetService(
			fakeLog,
			fakeTimeProvider,
			fakeGameWagerPlayerAccumRepository,
			fakeGameWagerStatCloseRepository,
			fakeGameWagerStatHourlyCloseRepository,
			fakeGameWagerStatDailyCloseRepository);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(gameWagerPlayerAccum.Count + minStatPlayerAccum.Count + gameWagerStatPlayerAccumHourly.Count + gameWagerStatPlayerAccumDaily.Count, actual.Count);
		Assert.Equal(gameWagerPlayerAccum.Bet + minStatPlayerAccum.Bet + gameWagerStatPlayerAccumHourly.Bet + gameWagerStatPlayerAccumDaily.Bet, actual.Bet);
		Assert.Equal(gameWagerPlayerAccum.ValidBet + minStatPlayerAccum.ValidBet + gameWagerStatPlayerAccumHourly.ValidBet + gameWagerStatPlayerAccumDaily.ValidBet, actual.ValidBet);
		Assert.Equal(gameWagerPlayerAccum.Settle + minStatPlayerAccum.Settle + gameWagerStatPlayerAccumHourly.Settle + gameWagerStatPlayerAccumDaily.Settle, actual.Settle);
		Assert.Equal(gameWagerPlayerAccum.Commission + minStatPlayerAccum.Commission + gameWagerStatPlayerAccumHourly.Commission + gameWagerStatPlayerAccumDaily.Commission, actual.Commission);
		Assert.Equal(gameWagerPlayerAccum.Turnover + minStatPlayerAccum.Turnover + gameWagerStatPlayerAccumHourly.Turnover + gameWagerStatPlayerAccumDaily.Turnover, actual.Turnover);
		Assert.Equal(gameWagerPlayerAccum.WinLoss + minStatPlayerAccum.WinLoss + gameWagerStatPlayerAccumHourly.WinLoss + gameWagerStatPlayerAccumDaily.WinLoss, actual.WinLoss);
	}

	[Fact]
	public async Task GameWagerPlayerAccum_Is_Null()
	{
		var fakeLog = NullLogger<GameWagerStatPlayerAccumGetService>.Instance;
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeGameWagerPlayerAccumRepository = Substitute.For<IGameWagerPlayerAccumRepository>();
		var fakeGameWagerStatCloseRepository = Substitute.For<IGameWagerStatCloseRepository>();
		var fakeGameWagerStatHourlyCloseRepository = Substitute.For<IGameWagerStatHourlyCloseRepository>();
		var fakeGameWagerStatDailyCloseRepository = Substitute.For<IGameWagerStatDailyCloseRepository>();

		var query = new GameWagerStatPlayerAccumQuery(
			"4ucmqplxb029");

		var currentTime = new DateTimeOffset(2024, 02, 23, 09, 30, 22, TimeSpan.FromHours(+08));
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		_ = fakeGameWagerPlayerAccumRepository.GetNewestAsync(
			playerCode: query.PlayerCode,
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerPlayerAccumModel?)null);

		var minStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		var minEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 00, currentTime.Offset);
		var gameWagerStatPlayerAccumMin = new GameWagerStatPlayerAccumModel(
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
			.Returns(gameWagerStatPlayerAccumMin);

		var dateOn = DateOnly.FromDateTime(currentTime.Date);
		var endTimeOn = TimeOnly.FromTimeSpan(TimeSpan.FromHours(currentTime.Hour));
		var gameWagerStatPlayerAccumHourly = new GameWagerStatPlayerAccumModel(
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
			dateOn: Arg.Is(dateOn),
			endTimeOn: Arg.Is(endTimeOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameWagerStatPlayerAccumHourly);

		var gameWagerStatPlayerAccumDaily = new GameWagerStatPlayerAccumModel(
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
			startDayOn: Arg.Is(DateOnly.FromDateTime(DateTimeOffset.UnixEpoch.ToOffset(TimeSpan.FromHours(+8)).Date)),
			endDayOn: Arg.Is(DateOnly.FromDateTime(currentTime.Date)),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameWagerStatPlayerAccumDaily);

		var sut = new GameWagerStatPlayerAccumGetService(
			fakeLog,
			fakeTimeProvider,
			fakeGameWagerPlayerAccumRepository,
			fakeGameWagerStatCloseRepository,
			fakeGameWagerStatHourlyCloseRepository,
			fakeGameWagerStatDailyCloseRepository);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(gameWagerStatPlayerAccumMin.Count + gameWagerStatPlayerAccumHourly.Count + gameWagerStatPlayerAccumDaily.Count, actual.Count);
		Assert.Equal(gameWagerStatPlayerAccumMin.Bet + gameWagerStatPlayerAccumHourly.Bet + gameWagerStatPlayerAccumDaily.Bet, actual.Bet);
		Assert.Equal(gameWagerStatPlayerAccumMin.ValidBet + gameWagerStatPlayerAccumHourly.ValidBet + gameWagerStatPlayerAccumDaily.ValidBet, actual.ValidBet);
		Assert.Equal(gameWagerStatPlayerAccumMin.Settle + gameWagerStatPlayerAccumHourly.Settle + gameWagerStatPlayerAccumDaily.Settle, actual.Settle);
		Assert.Equal(gameWagerStatPlayerAccumMin.Commission + gameWagerStatPlayerAccumHourly.Commission + gameWagerStatPlayerAccumDaily.Commission, actual.Commission);
		Assert.Equal(gameWagerStatPlayerAccumMin.Turnover + gameWagerStatPlayerAccumHourly.Turnover + gameWagerStatPlayerAccumDaily.Turnover, actual.Turnover);
		Assert.Equal(gameWagerStatPlayerAccumMin.WinLoss + gameWagerStatPlayerAccumHourly.WinLoss + gameWagerStatPlayerAccumDaily.WinLoss, actual.WinLoss);
	}

	[Fact]
	public async Task GameWagerStatPlayerAccum_Min_Is_Null()
	{
		var fakeLog = NullLogger<GameWagerStatPlayerAccumGetService>.Instance;
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeGameWagerPlayerAccumRepository = Substitute.For<IGameWagerPlayerAccumRepository>();
		var fakeGameWagerStatCloseRepository = Substitute.For<IGameWagerStatCloseRepository>();
		var fakeGameWagerStatHourlyCloseRepository = Substitute.For<IGameWagerStatHourlyCloseRepository>();
		var fakeGameWagerStatDailyCloseRepository = Substitute.For<IGameWagerStatDailyCloseRepository>();

		var query = new GameWagerStatPlayerAccumQuery(
			"4ucmqplxb029");

		var currentTime = new DateTimeOffset(2024, 02, 23, 09, 30, 22, TimeSpan.FromHours(+08));
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		var gameWagerPlayerAccum = new GameWagerPlayerAccumModel(
			PlayerCode: "4ucmqplxb029",
			AccumOn: new DateTimeOffset(2023, 10, 01, 00, 00, 00, TimeSpan.FromHours(+08)),
			Count: 1771,
			Bet: 44055.900000m,
			ValidBet: 42403.600000m,
			Settle: 0.00m,
			Commission: 0.00m,
			Turnover: 42376.900000m,
			WinLoss: -38458.265001m);
		_ = fakeGameWagerPlayerAccumRepository.GetNewestAsync(
			playerCode: query.PlayerCode,
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameWagerPlayerAccum);

		var minStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		var minEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 00, currentTime.Offset);
		_ = fakeGameWagerStatCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			startOn: Arg.Is(minStartOn),
			endOn: Arg.Is(minEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerStatPlayerAccumModel?)null);

		var dateOn = DateOnly.FromDateTime(currentTime.Date);
		var endTimeOn = TimeOnly.FromTimeSpan(TimeSpan.FromHours(currentTime.Hour));
		var gameWagerStatPlayerAccumHourly = new GameWagerStatPlayerAccumModel(
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
			dateOn: Arg.Is(dateOn),
			endTimeOn: Arg.Is(endTimeOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameWagerStatPlayerAccumHourly);

		var gameWagerStatPlayerAccumDaily = new GameWagerStatPlayerAccumModel(
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
			startDayOn: Arg.Is(DateOnly.FromDateTime(gameWagerPlayerAccum.AccumOn.ToOffset(TimeSpan.FromHours(+8)).Date)),
			endDayOn: Arg.Is(DateOnly.FromDateTime(currentTime.Date)),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameWagerStatPlayerAccumDaily);

		var sut = new GameWagerStatPlayerAccumGetService(
			fakeLog,
			fakeTimeProvider,
			fakeGameWagerPlayerAccumRepository,
			fakeGameWagerStatCloseRepository,
			fakeGameWagerStatHourlyCloseRepository,
			fakeGameWagerStatDailyCloseRepository);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(gameWagerPlayerAccum.Count + gameWagerStatPlayerAccumHourly.Count + gameWagerStatPlayerAccumDaily.Count, actual.Count);
		Assert.Equal(gameWagerPlayerAccum.Bet + gameWagerStatPlayerAccumHourly.Bet + gameWagerStatPlayerAccumDaily.Bet, actual.Bet);
		Assert.Equal(gameWagerPlayerAccum.ValidBet + gameWagerStatPlayerAccumHourly.ValidBet + gameWagerStatPlayerAccumDaily.ValidBet, actual.ValidBet);
		Assert.Equal(gameWagerPlayerAccum.Settle + gameWagerStatPlayerAccumHourly.Settle + gameWagerStatPlayerAccumDaily.Settle, actual.Settle);
		Assert.Equal(gameWagerPlayerAccum.Commission + gameWagerStatPlayerAccumHourly.Commission + gameWagerStatPlayerAccumDaily.Commission, actual.Commission);
		Assert.Equal(gameWagerPlayerAccum.Turnover + gameWagerStatPlayerAccumHourly.Turnover + gameWagerStatPlayerAccumDaily.Turnover, actual.Turnover);
		Assert.Equal(gameWagerPlayerAccum.WinLoss + gameWagerStatPlayerAccumHourly.WinLoss + gameWagerStatPlayerAccumDaily.WinLoss, actual.WinLoss);
	}

	[Fact]
	public async Task GameWagerStatPlayerAccum_Min_Is_Null_And_Hourly_Is_Null()
	{
		var fakeLog = NullLogger<GameWagerStatPlayerAccumGetService>.Instance;
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeGameWagerPlayerAccumRepository = Substitute.For<IGameWagerPlayerAccumRepository>();
		var fakeGameWagerStatCloseRepository = Substitute.For<IGameWagerStatCloseRepository>();
		var fakeGameWagerStatHourlyCloseRepository = Substitute.For<IGameWagerStatHourlyCloseRepository>();
		var fakeGameWagerStatDailyCloseRepository = Substitute.For<IGameWagerStatDailyCloseRepository>();

		var query = new GameWagerStatPlayerAccumQuery(
			"4ucmqplxb029");

		var currentTime = new DateTimeOffset(2024, 02, 23, 09, 30, 22, TimeSpan.FromHours(+08));
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		var gameWagerPlayerAccum = new GameWagerPlayerAccumModel(
			PlayerCode: "4ucmqplxb029",
			AccumOn: new DateTimeOffset(2023, 10, 01, 00, 00, 00, TimeSpan.FromHours(+08)),
			Count: 1771,
			Bet: 44055.900000m,
			ValidBet: 42403.600000m,
			Settle: 0.00m,
			Commission: 0.00m,
			Turnover: 42376.900000m,
			WinLoss: -38458.265001m);
		_ = fakeGameWagerPlayerAccumRepository.GetNewestAsync(
			playerCode: query.PlayerCode,
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameWagerPlayerAccum);

		var minStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		var minEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 00, currentTime.Offset);
		_ = fakeGameWagerStatCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			startOn: Arg.Is(minStartOn),
			endOn: Arg.Is(minEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerStatPlayerAccumModel?)null);

		var dateOn = DateOnly.FromDateTime(currentTime.Date);
		var endTimeOn = TimeOnly.FromTimeSpan(TimeSpan.FromHours(currentTime.Hour));
		_ = fakeGameWagerStatHourlyCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			dateOn: Arg.Is(dateOn),
			endTimeOn: Arg.Is(endTimeOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerStatPlayerAccumModel?)null);

		var gameWagerStatPlayerAccumDaily = new GameWagerStatPlayerAccumModel(
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
			startDayOn: Arg.Is(DateOnly.FromDateTime(gameWagerPlayerAccum.AccumOn.ToOffset(TimeSpan.FromHours(+8)).Date)),
			endDayOn: Arg.Is(DateOnly.FromDateTime(currentTime.Date)),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameWagerStatPlayerAccumDaily);

		var sut = new GameWagerStatPlayerAccumGetService(
			fakeLog,
			fakeTimeProvider,
			fakeGameWagerPlayerAccumRepository,
			fakeGameWagerStatCloseRepository,
			fakeGameWagerStatHourlyCloseRepository,
			fakeGameWagerStatDailyCloseRepository);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(gameWagerPlayerAccum.Count + gameWagerStatPlayerAccumDaily.Count, actual.Count);
		Assert.Equal(gameWagerPlayerAccum.Bet + gameWagerStatPlayerAccumDaily.Bet, actual.Bet);
		Assert.Equal(gameWagerPlayerAccum.ValidBet + gameWagerStatPlayerAccumDaily.ValidBet, actual.ValidBet);
		Assert.Equal(gameWagerPlayerAccum.Settle + gameWagerStatPlayerAccumDaily.Settle, actual.Settle);
		Assert.Equal(gameWagerPlayerAccum.Commission + gameWagerStatPlayerAccumDaily.Commission, actual.Commission);
		Assert.Equal(gameWagerPlayerAccum.Turnover + gameWagerStatPlayerAccumDaily.Turnover, actual.Turnover);
		Assert.Equal(gameWagerPlayerAccum.WinLoss + gameWagerStatPlayerAccumDaily.WinLoss, actual.WinLoss);
	}

	[Fact]
	public async Task GameWagerStatPlayerAccum_Min_Is_Null_And_Hourly_Is_Null_And_Daily_Is_Null()
	{
		var fakeLog = NullLogger<GameWagerStatPlayerAccumGetService>.Instance;
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeGameWagerPlayerAccumRepository = Substitute.For<IGameWagerPlayerAccumRepository>();
		var fakeGameWagerStatCloseRepository = Substitute.For<IGameWagerStatCloseRepository>();
		var fakeGameWagerStatHourlyCloseRepository = Substitute.For<IGameWagerStatHourlyCloseRepository>();
		var fakeGameWagerStatDailyCloseRepository = Substitute.For<IGameWagerStatDailyCloseRepository>();

		var query = new GameWagerStatPlayerAccumQuery(
			"4ucmqplxb029");

		var currentTime = new DateTimeOffset(2024, 02, 23, 09, 30, 22, TimeSpan.FromHours(+08));
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		var gameWagerPlayerAccum = new GameWagerPlayerAccumModel(
			PlayerCode: "4ucmqplxb029",
			AccumOn: new DateTimeOffset(2023, 10, 01, 00, 00, 00, TimeSpan.FromHours(+08)),
			Count: 1771,
			Bet: 44055.900000m,
			ValidBet: 42403.600000m,
			Settle: 0.00m,
			Commission: 0.00m,
			Turnover: 42376.900000m,
			WinLoss: -38458.265001m);
		_ = fakeGameWagerPlayerAccumRepository.GetNewestAsync(
			playerCode: query.PlayerCode,
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameWagerPlayerAccum);

		var minStartOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 00, 00, currentTime.Offset);
		var minEndOn = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 00, currentTime.Offset);
		_ = fakeGameWagerStatCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			startOn: Arg.Is(minStartOn),
			endOn: Arg.Is(minEndOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerStatPlayerAccumModel?)null);

		var dateOn = DateOnly.FromDateTime(currentTime.Date);
		var endTimeOn = TimeOnly.FromTimeSpan(TimeSpan.FromHours(currentTime.Hour));
		_ = fakeGameWagerStatHourlyCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			dateOn: Arg.Is(dateOn),
			endTimeOn: Arg.Is(endTimeOn),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerStatPlayerAccumModel?)null);

		_ = fakeGameWagerStatDailyCloseRepository.GetPlayerAccumAsync(
			playerCode: Arg.Is(query.PlayerCode),
			startDayOn: Arg.Is(DateOnly.FromDateTime(gameWagerPlayerAccum.AccumOn.ToOffset(TimeSpan.FromHours(+8)).Date)),
			endDayOn: Arg.Is(DateOnly.FromDateTime(currentTime.Date)),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns((GameWagerStatPlayerAccumModel?)null);

		var sut = new GameWagerStatPlayerAccumGetService(
			fakeLog,
			fakeTimeProvider,
			fakeGameWagerPlayerAccumRepository,
			fakeGameWagerStatCloseRepository,
			fakeGameWagerStatHourlyCloseRepository,
			fakeGameWagerStatDailyCloseRepository);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(gameWagerPlayerAccum.Count, actual.Count);
		Assert.Equal(gameWagerPlayerAccum.Bet, actual.Bet);
		Assert.Equal(gameWagerPlayerAccum.ValidBet, actual.ValidBet);
		Assert.Equal(gameWagerPlayerAccum.Settle, actual.Settle);
		Assert.Equal(gameWagerPlayerAccum.Commission, actual.Commission);
		Assert.Equal(gameWagerPlayerAccum.Turnover, actual.Turnover);
		Assert.Equal(gameWagerPlayerAccum.WinLoss, actual.WinLoss);
	}
}
