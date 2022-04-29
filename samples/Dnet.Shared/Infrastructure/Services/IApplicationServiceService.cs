using Dnet.App.Shared.Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dnet.App.Shared.Infrastructure.Services
{
    public interface IApplicationServiceService
    {
        Task<List<Person>> GetPersons();
    }
}
