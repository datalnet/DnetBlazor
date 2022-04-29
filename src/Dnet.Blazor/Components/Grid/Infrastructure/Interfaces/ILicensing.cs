namespace Dnet.Blazor.Components.Grid.Infrastructure.Interfaces
{
    public interface ILicensing
    {
        public string Decrypt(string license, string privateKey);
    }
}
