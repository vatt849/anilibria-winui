using anilibria.Exceptions;
using anilibria.Models;
using anilibria.Models.Request;
using Flurl;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace anilibria.Common
{
    public class Anilibria
    {
        const string API_BASE_V3 = @"https://api.anilibria.tv/v3";

        private readonly HttpService service = new();

        public async Task<PaginatedReleasesResponse> GetReleases(int Limit = 5)
        {
            var url = new Url(API_BASE_V3)
                .AppendPathSegment("/title/updates")
                .SetQueryParams(new
                {
                    limit = Limit,
                    filter = "id,code,names,posters,genres,description"
                });

            Debug.WriteLine($"get releases by limit: {url}");

            var response = await service.GetAsync(url);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var errResp = JsonSerializer.Deserialize<ErrorResponse>(response.Content);

                throw new ApiException(errResp.Error.Message, errResp.Error.Code);
            }

            return JsonSerializer.Deserialize<PaginatedReleasesResponse>(response.Content);
        }

        public async Task<PaginatedReleasesResponse> GetReleases(int Limit = 20, int Page = 1)
        {
            var url = new Url(API_BASE_V3)
                .AppendPathSegment("/title/updates")
                .SetQueryParams(new
                {
                    items_per_page = Limit,
                    page = Page,
                    filter = "id,code,names,posters,genres,description"
                });

            Debug.WriteLine($"get releases by page: {url}");

            var response = await service.GetAsync(url);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var errResp = JsonSerializer.Deserialize<ErrorResponse>(response.Content);

                throw new ApiException(errResp.Error.Message, errResp.Error.Code);
            }

            return JsonSerializer.Deserialize<PaginatedReleasesResponse>(response.Content);
        }

        public async Task<PaginatedReleasesResponse> Search(string Query, int Limit = 10)
        {
            var url = new Url(API_BASE_V3)
                .AppendPathSegment("/title/search")
                .SetQueryParams(new
                {
                    search = Query,
                    limit = Limit,
                    filter = "id,code,names,posters"
                });

            Debug.WriteLine($"search: {url}");

            var response = await service.GetAsync(url);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var errResp = JsonSerializer.Deserialize<ErrorResponse>(response.Content);

                throw new ApiException(errResp.Error.Message, errResp.Error.Code);
            }

            return JsonSerializer.Deserialize<PaginatedReleasesResponse>(response.Content);
        }

        public async Task<Release> GetRelease(int id)
        {
            var url = new Url(API_BASE_V3)
                .AppendPathSegment("/title")
                .SetQueryParams(new
                {
                    id
                });

            Debug.WriteLine($"get: {url}");

            var response = await service.GetAsync(url);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var errResp = JsonSerializer.Deserialize<ErrorResponse>(response.Content);

                throw new ApiException(errResp.Error.Message, errResp.Error.Code);
            }

            return JsonSerializer.Deserialize<Release>(response.Content);
        }
    }
}
