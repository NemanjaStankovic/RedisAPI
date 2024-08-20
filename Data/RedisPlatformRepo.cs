using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RedisAPI.Data;
using RedisAPI.Models;
using SignalRChat.Hubs;
using StackExchange.Redis;

namespace RedisAPI
{
    public class RedisPlatfromRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly DataContext _context;
        private readonly IHubContext<PlatformHub> _hubContext;

        public RedisPlatfromRepo(IConnectionMultiplexer redis, DataContext context, IHubContext<PlatformHub> hubContext)
        {
            _redis = redis;
            _context = context;
            _hubContext = hubContext;
        }
        public void CreatePlatfrom()
        {

            IDatabase db = _redis.GetDatabase();

            var igrice = _context.Igrice.ToList();

            if(igrice == null)
            {
                Console.WriteLine("Nema nista");
            }

            foreach(var igrica in igrice)
            {
                string id = igrica.Id.ToString();
                string naziv = igrica.Naziv;
                string cena = igrica.Cena.ToString();
                string onSale = igrica.onSale.ToString();
                string novaCena = igrica.novaCena.ToString();
                

                if (db.HashExists("id:" + id, "naziv"))
                {

                    _redis.GetSubscriber().Publish("id:" + id, $"{id}");
                

                    db.HashSet("id:" + id, new HashEntry[] { new HashEntry("sale", $"{onSale}"), new HashEntry("novaCena", $"{novaCena}") } );

                    
                }
                else{

                    db.HashSet("id:" + id,new HashEntry [] {new HashEntry("naziv", $"{naziv}"), new HashEntry("cena", $"{cena}"), new HashEntry("sale", $"{onSale}"), new HashEntry("novaCena", $"{novaCena}")});
                
                }
            }

            SendSaleNotificationsAsync();//User = Nedza
            
        }


        public IEnumerable<Igrica?>? preuzmiSveIgrice()
        {
            var db = _redis.GetDatabase();
            EndPoint endPoint = _redis.GetEndPoints().First();
            RedisKey[] keys = _redis.GetServer(endPoint).Keys(pattern: "id:*").ToArray();
            List<Igrica> list = new List<Igrica>();

            foreach (var key in keys)
            {
                Igrica igra = new Igrica();

                string pom = key.ToString();

                string[] parts = pom.Split(':');
                
                igra.Id = Int32.Parse(parts[1]);
                
                igra.Naziv = db.HashGet(key, "naziv");
                igra.Cena = (int)db.HashGet(key, "cena");
                var popust = db.HashGet(key, "sale");
                if(popust == "True")
                {
                    igra.onSale = true;
                }
                else{
                    igra.onSale = false;
                }
                igra.novaCena = (int)db.HashGet(key, "novaCena");


                list.Add(igra);
            }
            IEnumerable<Igrica> igrice = list;

            return igrice;
        }

        public bool postojiIgrica(string id, string username)
        {
            var user = _context.Users.Where(p => p.Username == username).FirstOrDefault();

            if(user == null)
                return false;
                
            var igrice = user.Wishlist;

            if (igrice == null)
                return false;

            var listaIgrica = igrice.Split(";").ToList();

            if (listaIgrica.Contains(id))
            {
                return true;
            }
            
            return false;
        }

        public string preuzmiIgricu(string id)
        {
            var db = _redis.GetDatabase();
            
            EndPoint endPoint = _redis.GetEndPoints().First();
            RedisKey[] keys = _redis.GetServer(endPoint).Keys(pattern: "id:*").ToArray();


            foreach (var key in keys)
            {
            
                string pom = key.ToString();

                string[] parts = pom.Split(':');

                if(parts[1] == id)
                {
                    string n= db.HashGet(key, "naziv");

                    return n;
                }

            }

            return "ne";
        }

        public IEnumerable<Igrica?>? preuzmiOmiljeneIgrice(string[] wishlist)
        {
            var db = _redis.GetDatabase();
            EndPoint endPoint = _redis.GetEndPoints().First();
            RedisKey[] keys = _redis.GetServer(endPoint).Keys(pattern: "id:*").ToArray();
            List<Igrica> lista = new List<Igrica>();

            foreach(var key in keys)
            {

                foreach(var gmid in wishlist)
                {                    
                    
                    string pom = key.ToString();

                    string[] parts = pom.Split(':');

                    if(gmid == parts[1])
                    {
                        Igrica igra = new Igrica();

                        igra.Id = Int32.Parse(parts[1]);

                        igra.Naziv = db.HashGet(key, "naziv");;
                        igra.Cena = (int)db.HashGet(key, "cena");
                        var popust = db.HashGet(key, "sale");
                        if(popust == "True")
                        {
                            igra.onSale = true;
                        }
                        else
                        {
                            igra.onSale = false;
                        }
                        igra.novaCena = (int)db.HashGet(key, "novaCena");

                        lista.Add(igra);
                    }
                }
            }

            IEnumerable<Igrica> igrice = lista;

            return igrice;
        }

        public void unsubscribeAll()
        {
            _redis.GetSubscriber().UnsubscribeAll();
        }
        
        public void SendSaleNotificationsAsync()
        {
            var igriceOnSale = _context.Igrice.Where(i => i.onSale).ToList();

            var pubsub = _redis.GetSubscriber();

            foreach (var igra in igriceOnSale)
            {
                pubsub.Subscribe("id:" + igra.Id, (channel, message) =>
                {
                    _hubContext.Clients.All.SendAsync("ReceiveMessage", message.ToString());
                });
            }
        }
    }
}