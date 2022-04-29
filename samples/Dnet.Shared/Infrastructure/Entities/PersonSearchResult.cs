using System.Collections.Generic;

namespace Dnet.App.Shared.Infrastructure.Entities
{
    public class PersonSearchResult
    {
        public List<Person> Persons { get; set; } = new List<Person>();

        public int TotalItems { get; set; }

    }
}
