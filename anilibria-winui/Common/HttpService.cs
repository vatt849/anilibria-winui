﻿using System;
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

        public async Task<string> GetAsync(string uri)
        {
            using HttpResponseMessage response = await _client.GetAsync(uri);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string uri, string data, string contentType)
        {
            using HttpContent content = new StringContent(data, Encoding.UTF8, contentType);

            HttpRequestMessage requestMessage = new()
            {
                Content = content,
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri)
            };

            using HttpResponseMessage response = await _client.SendAsync(requestMessage);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
