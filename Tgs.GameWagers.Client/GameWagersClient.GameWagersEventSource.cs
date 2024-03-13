using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tgs.GameWagers.Common;

namespace Tgs.GameWagers.Client;
internal partial class GameWagersClient
{
	public IAsyncEnumerable<GameWagersEventSourceInfo> GameWagersEventSourceQueryAllAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default)
	{
		var path = new Uri(_httpClient.BaseAddress!, $"/game-wagers-event-source/all/id/{gameProviderId}");
		var builder = new UriBuilder(path);
		var query = HttpUtility.ParseQueryString(builder.Query);
		query["id"] = id.ToString();
		query["limit"] = limit.ToString();
		builder.Query = query.ToString();
		string url = builder.ToString();

		return _httpClient.GetAsync(url, cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<IEnumerable<GameWagersEventSourceInfo>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable()
				.SelectMany(arr => arr!.ToAsyncEnumerable()))
			.Where(info => info is not null)
			.Select(info => info!)
			.ToEnumerable()
			.ToAsyncEnumerable();
	}
	public IAsyncEnumerable<GameWagersEventSourceInfo> GameWagersEventSourceQuerySettlementsAsync(Guid gameProviderId, Guid id, int limit = 100, CancellationToken cancellationToken = default)
	{
		var path = new Uri(_httpClient.BaseAddress!, $"/game-wagers-event-source/settlements/id/{gameProviderId}");
		var builder = new UriBuilder(path);
		var query = HttpUtility.ParseQueryString(builder.Query);
		query["id"] = id.ToString();
		query["limit"] = limit.ToString();
		builder.Query = query.ToString();
		string url = builder.ToString();

		return _httpClient.GetAsync(url, cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<IEnumerable<GameWagersEventSourceInfo>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable()
				.SelectMany(arr => arr!.ToAsyncEnumerable()))
			.Where(info => info is not null)
			.Select(info => info!)
			.ToEnumerable()
			.ToAsyncEnumerable();
	}
	public IAsyncEnumerable<GameWagersEventSourceInfo> GameWagersEventSourceQuerySettlementsAsync(Guid gameProviderId, DateTimeOffset timepoint, int limit = 100, CancellationToken cancellationToken = default)

	{
		var path = new Uri(_httpClient.BaseAddress!, $"/game-wagers-event-source/settlements/timepoint/{gameProviderId}");
		var builder = new UriBuilder(path);
		var query = HttpUtility.ParseQueryString(builder.Query);
		query["timepoint"] = timepoint.ToString("u");
		query["limit"] = limit.ToString();
		builder.Query = query.ToString();
		string url = builder.ToString();

		return _httpClient.GetAsync(url, cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<IEnumerable<GameWagersEventSourceInfo>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable()
				.SelectMany(arr => arr!.ToAsyncEnumerable()))
			.Where(info => info is not null)
			.Select(info => info!)
			.ToEnumerable()
			.ToAsyncEnumerable();
	}
	public IAsyncEnumerable<GameWagersEventSourceInfo> GameWagersEventSourceQuerySettlementsAsync(Guid gameProviderId, DateTimeOffset fromOn, DateTimeOffset toOn, int page = 1, int pageSize = 100, CancellationToken cancellationToken = default)

	{
		var path = new Uri(_httpClient.BaseAddress!, $"/game-wagers-event-source/settlements/time-interval/{gameProviderId}");
		var builder = new UriBuilder(path);
		var query = HttpUtility.ParseQueryString(builder.Query);
		query["fromOn"] = fromOn.ToString("u");
		query["toOn"] = toOn.ToString("u");
		query["page"] = page.ToString();
		query["pageSize"] = pageSize.ToString();
		builder.Query = query.ToString();
		string url = builder.ToString();

		return _httpClient.GetAsync(url, cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<IEnumerable<GameWagersEventSourceInfo>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable()
				.SelectMany(arr => arr!.ToAsyncEnumerable()))
			.Where(info => info is not null)
			.Select(info => info!)
			.ToEnumerable()
			.ToAsyncEnumerable();
	}
}
