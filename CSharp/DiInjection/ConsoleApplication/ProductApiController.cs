using System.Net.WebSockets;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CSharp.DiInjection.ConsoleApplication
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : ControllerBase
    {
        // DI HttpClientFactory
        private IHttpClientFactory _httpFactory {get; set; }

        public ProductApiController(IHttpClientFactory httpFactory)
        {
            // DI HttpClientFactory
            _httpFactory = httpFactory;
        }
        
        public async Task<string> RequestHomepage()
        {
            var request  = new HttpRequestMessage(HttpMethod.Get, "https://www.google.com");
            var client = this._httpFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return $@"StatusCode: {response.StatusCode}";
            }

        }
    }
}