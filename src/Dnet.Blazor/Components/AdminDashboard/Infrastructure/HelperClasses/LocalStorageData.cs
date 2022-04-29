using System.Collections.Generic;

namespace Dnet.Blazor.Components.AdminDashboard.Infrastructure.HelperClasses
{
    public class LocalStorageData
    {

        public CurrentUser CurrentUser { get; set; }

        public Rights Rights { get; set; }

        public bool IsMinified { get; set; }
    }

    public class CurrentUser
    {

        public string UserName { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public bool IsInGodRole { get; set; }

        public string Token { get; set; }
    }

    public class Rights
    {
        public List<string> Values { get; set; } = new List<string>();
    }
}
