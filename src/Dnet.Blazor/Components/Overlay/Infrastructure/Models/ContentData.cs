namespace Dnet.Blazor.Components.Overlay.Infrastructure.Models
{
    public class ContentData
    {
        private object _value;

        public void SetValue<T>(T value)
        {
            _value = value;
        }

        public T GetValue<T>()
        {
            return (T)_value;
        }
    }
}
