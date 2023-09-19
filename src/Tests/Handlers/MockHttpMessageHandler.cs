using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace Tests.Handlers
{
    public interface IMockHttpMessageHandler
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }

    public class MockHttpMessageHandler: HttpMessageHandler
    {
        private readonly IMockHttpMessageHandler _mockHandler;

        public MockHttpMessageHandler(IMockHttpMessageHandler mockHandler)
        {
            _mockHandler =mockHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await _mockHandler.SendAsync(request, cancellationToken);
        }
    }
}