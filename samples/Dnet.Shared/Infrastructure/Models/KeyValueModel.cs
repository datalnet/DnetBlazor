namespace Dnet.App.Shared.Infrastructure.Models
{
    public class KeyValueModel<TKey>
    {
        public TKey Key { get; set; }

        public string Value { get; set; }
    }
}
