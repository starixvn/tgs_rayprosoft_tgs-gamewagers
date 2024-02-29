using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using Tgs.GameWagers.Common;

namespace Tgs.GameWagers.Client;

internal partial class GameWagersClient : IGameWagersClient
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters =
		{
			new JsonStringEnumConverter()
		}
	};

	private readonly HttpClient _httpClient;

	public GameWagersClient(HttpClient httpClient)
		=> _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

	public IAsyncEnumerable<BrandWagerGameStatsInfo> BrandWagerGameStatsQueryAsync(
		string brandCode,
		DateTimeOffset startOn,
		DateTimeOffset endOn,
		BrandWagerGameMinsStatsSort sort,
		bool isAscending = true,
		int page = 1,
		int pageSize = 1000,
		CancellationToken cancellationToken = default)
	{
		var path = new Uri(_httpClient.BaseAddress!, $"/api/brandwagergameminsstats/{brandCode}");
		var builder = new UriBuilder(path);
		var query = HttpUtility.ParseQueryString(builder.Query);
		query["startOn"] = startOn.ToString("u");
		query["endOn"] = endOn.ToString("u");
		query["sort"] = sort.ToString();
		query["isAscending"] = isAscending.ToString();
		query["page"] = page.ToString();
		query["pageSize"] = pageSize.ToString();
		builder.Query = query.ToString();
		string url = builder.ToString();

		return _httpClient.GetAsync(url, cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<IEnumerable<BrandWagerGameStatsInfo>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable()
				.SelectMany(arr => arr!.ToAsyncEnumerable()))
			.Where(info => info is not null)
			.Select(info => info!)
			.ToEnumerable()
			.ToAsyncEnumerable();
	}

	public IAsyncEnumerable<BrandWagerPlayerStatsInfo> BrandWagerPlayerStatsQueryAsync(
		string brandCode,
		DateTimeOffset startOn,
		DateTimeOffset endOn,
		BrandWagerPlayerMinsStatsSort sort,
		bool isAscending = true,
		int page = 1,
		int pageSize = 1000,
		CancellationToken cancellationToken = default)
	{
		var path = new Uri(_httpClient.BaseAddress!, $"/api/brandwagerplayerminsstats/{brandCode}");
		var builder = new UriBuilder(path);
		var query = HttpUtility.ParseQueryString(builder.Query);
		query["startOn"] = startOn.ToString("u");
		query["endOn"] = endOn.ToString("u");
		query["sort"] = sort.ToString();
		query["isAscending"] = isAscending.ToString();
		query["page"] = page.ToString();
		query["pageSize"] = pageSize.ToString();
		builder.Query = query.ToString();
		string url = builder.ToString();

		return _httpClient.GetAsync(url, cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<IEnumerable<BrandWagerPlayerStatsInfo>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable()
				.SelectMany(arr => arr!.ToAsyncEnumerable()))
			.Where(info => info is not null)
			.Select(info => info!)
			.ToEnumerable()
			.ToAsyncEnumerable();
	}

	public IAsyncEnumerable<BrandWagerPlayerStatsInfo> BrandWagerPlayerStatsPlayerQueryAsync(
		string brandCode,
		DateTimeOffset startOn,
		DateTimeOffset endOn,
		IEnumerable<string> playerCodes,
		CancellationToken cancellationToken = default)
	{
		var path = new Uri(_httpClient.BaseAddress!, $"/api/brandwagerplayerminsstats/{brandCode}/playerCodes");
		var builder = new UriBuilder(path);
		var query = HttpUtility.ParseQueryString(builder.Query);
		query["startOn"] = startOn.ToString("u");
		query["endOn"] = endOn.ToString("u");

		foreach (var item in playerCodes.Select((playerCode, index) => new { PlayerCode = playerCode, Index = index }))
		{
			query[$"playerCode[{item.Index}]"] = item.PlayerCode;
		}

		builder.Query = query.ToString();
		string url = builder.ToString();

		return _httpClient.GetAsync(url, cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<IEnumerable<BrandWagerPlayerStatsInfo>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable()
				.SelectMany(arr => arr!.ToAsyncEnumerable()))
			.Where(info => info is not null)
			.Select(info => info!)
			.ToEnumerable()
			.ToAsyncEnumerable();
	}

	public async ValueTask<GameWagerStatPlayerAccumInfo> GameWagerStatGetPlayerAccumAsync(string playerCode, CancellationToken cancellationToken = default)
		=> await _httpClient.GetAsync($"/stat/{playerCode}/accum", cancellationToken)
		.ToAsyncEnumerable()
		.Do(response => response.EnsureSuccessStatusCode())
		.Where(response => response.StatusCode == System.Net.HttpStatusCode.OK)
		.SelectMany(response => response.Content
			.ReadFromJsonAsync<GameWagerStatPlayerAccumInfo>(_SerializerOptions, cancellationToken)
			.ToAsyncEnumerable())
		.FirstOrDefaultAsync(cancellationToken: cancellationToken)
		.ConfigureAwait(false)
		?? throw new NullReferenceException($"This player's VIP Level does not exist.{playerCode}");
}
