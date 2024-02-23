using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using OpenTelemetry.Exporter;
using Tgs.GameWagers.Common;
using Tgs.GameWagers.Core;
using Tgs.GameWagers.Core.ApplicationServices;
using Tgs.ServiceUtility;
using Tgs.VipLevelCommand.IntegrationTest;

namespace Tgs.GameWagers.Client.IntegrationTest;

public class GameWagersClientUnitTest
{
    [Fact]
    public async Task Brand_Wager_Game_Stats_Query()
    {
        var brandCode = "PB";
        var startOn = DateTimeOffset.UtcNow.AddHours(-1);
        var endOn = DateTimeOffset.UtcNow;
        var sort = BrandWagerGameMinsStatsSort.WinLoss;
        var isAscending = false;
        var page = 1;
        var pageSize = 1000;

        var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IQueryService<BrandWagerGameStatsQuery, BrandWagerGameStatsReslut>>();
            var query = new BrandWagerGameStatsQuery(
                BrandCode: brandCode,
                StartOn: startOn,
                EndOn: endOn,
                Sort: sort,
                IsAscending: isAscending,
                Page: page,
                PageSize: pageSize);
            _ = fakeService
            .QueryAsync(
                query: Arg.Is(query),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Enumerable.Empty<BrandWagerGameStatsReslut>().ToAsyncEnumerable());

            _ = services.AddSingleton(fakeService);
        });
        var httpClient = web.CreateClient();

        var sut = new GameWagersClient(httpClient);

        _ = await sut
            .BrandWagerGameStatsQueryAsync(
            brandCode: brandCode,
            startOn: startOn,
            endOn: endOn,
            sort: sort,
            isAscending: isAscending,
            page: page,
            pageSize: pageSize)
            .ToArrayAsync()
            ?? Enumerable.Empty<BrandWagerGameStatsInfo>();
    }

    [Fact]
    public async Task Brand_Wager_Player_Stats_Query()
    {
        var brandCode = "PB";
        var startOn = DateTimeOffset.UtcNow.AddHours(-1);
        var endOn = DateTimeOffset.UtcNow;
        var sort = BrandWagerPlayerMinsStatsSort.WinLoss;
        var isAscending = false;
        var page = 1;
        var pageSize = 1000;

        var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IQueryService<BrandWagerPlayerStatsQuery, BrandWagerPlayerStatsReslut>>();

            var query = new BrandWagerPlayerStatsQuery(
                BrandCode: brandCode,
                StartOn: startOn,
                EndOn: endOn,
                Sort: sort,
                IsAscending: isAscending,
                Page: page,
                PageSize: pageSize);
            _ = fakeService
            .QueryAsync(
                query: query,
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Enumerable.Empty<BrandWagerPlayerStatsReslut>().ToAsyncEnumerable());

            _ = services.AddSingleton(fakeService);
        });
        var httpClient = web.CreateClient();

        var sut = new GameWagersClient(httpClient);

        _ = await sut
            .BrandWagerPlayerStatsQueryAsync(
            brandCode: brandCode,
            startOn: startOn,
            endOn: endOn,
            sort: sort,
            isAscending: isAscending,
            page: page,
            pageSize: pageSize)
            .ToArrayAsync()
            ?? Enumerable.Empty<BrandWagerPlayerStatsInfo>();
    }

    [Fact]
    public async Task Brand_Wager_Player_Stats_Player_Query()
    {
        var brandCode = "PB";
        var startOn = DateTimeOffset.UtcNow.AddHours(-1);
        var endOn = DateTimeOffset.UtcNow;
        var playerCodes = new string[] { "test1", "test2" };

        var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IBrandWagerPlayerMinsStatsRepository>();

            _ = fakeRepository
            .PlayerCodesQueryAsync(
                brandCode: Arg.Is(brandCode),
                startOn: Arg.Is(startOn),
                endOn: Arg.Is(endOn),
                playerCodes: Arg.Is(playerCodes),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Enumerable.Empty<BrandWagerPlayerStatsInfo>().ToAsyncEnumerable());

            _ = services.AddSingleton(fakeRepository);
        });
        var httpClient = web.CreateClient();

        var sut = new GameWagersClient(httpClient);

        _ = await sut
            .BrandWagerPlayerStatsPlayerQueryAsync(
            brandCode: brandCode,
            startOn: startOn,
            endOn: endOn,
            playerCodes: playerCodes)
            .ToArrayAsync()
            ?? Enumerable.Empty<BrandWagerPlayerStatsInfo>();
    }

    [Fact]
    public async Task Game_Wager_Stat_Ge_Player_Accum()
    {
        var playerCode = "4ucmqplxb029";



        var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IGetService<GameWagerStatPlayerAccumQuery, GameWagerStatPlayerAccumInfo>>();

            var query = new GameWagerStatPlayerAccumQuery(
                PlayerCode: playerCode);
            var info = new GameWagerStatPlayerAccumInfo(
                PlayerCode: playerCode,
                Count: 100,
                Bet: 10000,
                ValidBet: 10000,
                Settle: 20000,
                Commission: 0,
                Turnover: 10000,
                WinLoss: 10000);

            _ = fakeService.GetAsync(
                query: Arg.Is(query),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(info);

            _ = services.AddSingleton(fakeService);
        });
        var httpClient = web.CreateClient();

        var sut = new GameWagersClient(httpClient);

        var actual = await sut.GameWagerStatGetPlayerAccumAsync(playerCode);

        Assert.NotNull(actual);
        Assert.Equal(100, actual.Count);
        Assert.Equal(10000.00m, actual.Bet);
        Assert.Equal(10000.00m, actual.ValidBet);
        Assert.Equal(20000.00m, actual.Settle);
        Assert.Equal(0.00m, actual.Commission);
        Assert.Equal(10000.00m, actual.Turnover);
        Assert.Equal(10000.00m, actual.WinLoss);
    }

    private static WebApplicationFactory<Program> CreateTestSettingSite(Action<IServiceCollection>? configureServices = null)
        => new GameWagersClientServiceApplication(hostBuilder => _ = hostBuilder
            .ConfigureAppConfiguration((_, configBuilder) => configBuilder.Sources.Clear())
            .ConfigureServices(services =>
            {
                _ = services
                .AddSingleton<IConfigureOptions<OtlpExporterOptions>>(
                    sp => new ConfigureNamedOptions<OtlpExporterOptions>(
                        Options.DefaultName,
                        exporterOptions => exporterOptions.TimeoutMilliseconds = 0));

                configureServices?.Invoke(services);
            }));
}
