using System.Net.Http.Json;
using System.Web;
using Tgs.GameWagers.Common;

namespace Tgs.GameWagers.Client;

internal partial class GameWagersClient
{
	public async ValueTask<GameWagerFirstInfo?> GameWagerFirstGetAsync(long wagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default)
	{
		var path = new Uri(_httpClient.BaseAddress!, $"/game-wager-first/{wagerId}");
		var builder = new UriBuilder(path);
		var query = HttpUtility.ParseQueryString(builder.Query);
		query["createdOn"] = createdOn.ToString("u");

		builder.Query = query.ToString();
		string url = builder.ToString();

		return await _httpClient.GetAsync(url.ToString(), cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.Where(response => response.StatusCode == System.Net.HttpStatusCode.OK)
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<GameWagerFirstInfo>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable())
			.FirstOrDefaultAsync(cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<GameWagerFirstInfo?> GameWagerFirstGetAsync(Guid gameProviderId, int gameId, Guid walletId, string externalWagerId, DateTimeOffset createdOn, CancellationToken cancellationToken = default)
	{
		var path = new Uri(_httpClient.BaseAddress!, $"/game-wager-first/{gameProviderId}/{gameId}/{walletId}/{externalWagerId}");
		var builder = new UriBuilder(path);
		var query = HttpUtility.ParseQueryString(builder.Query);
		query["createdOn"] = createdOn.ToString("u");

		builder.Query = query.ToString();
		string url = builder.ToString();

		return await _httpClient.GetAsync(url.ToString(), cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.Where(response => response.StatusCode == System.Net.HttpStatusCode.OK)
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<GameWagerFirstInfo>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable())
			.FirstOrDefaultAsync(cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}
}
