using RedisAPI.Models;

namespace RedisAPI.Data
{
    public interface IPlatformRepo
    {
        void CreatePlatfrom();

        IEnumerable<Igrica?>? preuzmiSveIgrice();

        string preuzmiIgricu(string id);

        IEnumerable<Igrica?>? preuzmiOmiljeneIgrice(string[] wishlist);

        void unsubscribeAll();

        void SendSaleNotificationsAsync();

        bool postojiIgrica(string id, string username);
    }
}