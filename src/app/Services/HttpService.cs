using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace app.Services
{
    public interface IHttpService
    {
        HttpResponseMessage Get(string url);
        HttpResponseMessage Post(string url, HttpContent content);
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
        Task<HttpResponseMessage> PostAsJsonAsync(string url, string contentJson);
    }

    public class HttpService: IHttpService
    {
        private readonly HttpClient _client;

        public HttpService(): this(new HttpClientHandler())
        {
        }

        public HttpService(HttpMessageHandler messageHandler)
        {
            _client = new HttpClient(messageHandler);
        }

        public HttpResponseMessage Get(string url)
        {
            return GetAsync(url).Result;
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return _client.GetAsync(url);
        }

        public HttpResponseMessage Post(string url, HttpContent content)
        {
            return PostAsync(url, content).Result;
        }

        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return _client.PostAsync(url, content);
        }

        public Task<HttpResponseMessage> PostAsJsonAsync(string url, string contentJson)
        {
            return _client.PostAsJsonAsync(url, contentJson);
        }
    }
}