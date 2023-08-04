using Dnet.App.Shared.Infrastructure.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Dnet.App.Shared.Infrastructure.Services;

public class PersonService: IPersonService
{
    private readonly IHttpClientFactory _clientFactory;

    public PersonService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<List<Person>> GetPersons()
    {
        var client = _clientFactory.CreateClient("WebHostURL");

        var dataPersons = await client.GetFromJsonAsync<List<Person>>("/sample-data/person_500.json");

        return dataPersons;
    }
}

public interface IPersonService
{
    Task<List<Person>> GetPersons();
}
