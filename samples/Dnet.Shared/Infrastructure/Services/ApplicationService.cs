using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dnet.App.Shared.Infrastructure.Entities;
using Microsoft.JSInterop;

namespace Dnet.App.Shared.Infrastructure.Services
{
    public class ApplicationServiceService : IApplicationServiceService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IJSRuntime _jSRuntime;

        public ApplicationServiceService(IHttpClientFactory clientFactory, IJSRuntime jSRuntime)
        {
            _clientFactory = clientFactory;
            _jSRuntime = jSRuntime;
        }

        public async Task<List<Person>> GetPersons()
        {
            var client = _clientFactory.CreateClient("WebHostURL");

            var request = new HttpRequestMessage
            {
                Method = new HttpMethod("Get"),
                RequestUri = new Uri("sample-data/person_500.json", UriKind.Relative),
            };

            Console.WriteLine(request.RequestUri);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<List<Person>>();

            //var apiREsponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

            throw new InvalidOperationException("errors");
        }
    }
}
