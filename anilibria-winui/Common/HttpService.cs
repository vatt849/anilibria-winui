using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace anilibria.Common
{
    public class HttpService
    {
        private readonly HttpClient _client;

        public HttpService()
        {
            HttpClientHandler handler = new()
            {
                AutomaticDecompression = DecompressionMethods.All
            };

            _client = new HttpClient(handler);
        }

        public async Task<HttpResponse> GetAsync(string uri)
        {
            using HttpResponseMessage response = await _client.GetAsync(uri);

            return new HttpResponse()
            {
                Content = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode
            };
        }

        public async Task<HttpResponse> PostAsync(string uri, string data, string contentType)
        {
            using HttpContent content = new StringContent(data, Encoding.UTF8, contentType);

            HttpRequestMessage requestMessage = new()
            {
                Content = content,
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri)
            };

            using HttpResponseMessage response = await _client.SendAsync(requestMessage);

            return new HttpResponse()
            {
                Content = await response.Content.ReadAsStringAsync(),
                StatusCode = response.StatusCode
            };
        }

        public string ToQueryString(NameValueCollection nvc)
        {
            var array = (
                from key in nvc.AllKeys
                from value in nvc.GetValues(key)
                select string.Format(
                    "{0}={1}",
                    key,
                    value
                )
                //HttpUtility.UrlEncode(key),
                //HttpUtility.UrlEncode(value))
                ).ToArray();

            return "?" + string.Join("&", array);
        }
    }

    public class HttpResponse
    {
        public string Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
