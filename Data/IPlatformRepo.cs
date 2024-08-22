using RedisAPI.Models;

namespace RedisAPI.Data
{
    public interface IPlatformRepo
    {
        void CreatePlatfrom();//bitna

        IEnumerable<Igrica?>? preuzmiSveIgrice();

        string preuzmiIgricu(string id);

        IEnumerable<Igrica?>? preuzmiOmiljeneIgrice(string[] wishlist);

        void unsubscribeAll();//ova

        void SendSaleNotificationsAsync();//ova

        bool postojiIgrica(string id, string username);
    }
}