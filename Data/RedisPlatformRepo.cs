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
            SendSaleNotificationsAsync();

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
                
                // nestooo:ID  
                if (db.HashExists("id:" + id, "naziv"))
                {

                    _redis.GetSubscriber().Publish("id:" + id, $"{id}");//u tubu
                

                    db.HashSet("id:" + id, new HashEntry[] {new HashEntry("naziv", $"{naziv}"), new HashEntry("cena", $"{cena}"), new HashEntry("sale", $"{onSale}"), new HashEntry("novaCena", $"{novaCena}") } );
                    
                }
                else{//puni redis kes bazu

                    db.HashSet("id:" + id,new HashEntry [] {new HashEntry("naziv", $"{naziv}"), new HashEntry("cena", $"{cena}"), new HashEntry("sale", $"{onSale}"), new HashEntry("novaCena", $"{novaCena}")});
                
                }
                //id:1  publish    1-------------------
                //id:2  publish   2-------------------- 
                //id:3  publish   3--------------------
                  
            }

            //SendSaleNotificationsAsync();//User = Nedza
            //odradi1 0 30 60 90 120 tu: 0 30 60 90 120 puni tubu
            //odradi2 p                   15 45 75 105 prazni tubu
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


            foreach (var key in keys)//key = id:1
            {
            
                string pom = key.ToString();
                //id:2
                string[] parts = pom.Split(':');//id:1 -> part[0] = id, part[1] = 1

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
            var igriceOnSale = _context.Igrice.Where(i => i.onSale).ToList();//1,3

            var pubsub = _redis.GetSubscriber();//uzima subsciber operatera

            foreach (var igra in igriceOnSale)//1,3
            {
                pubsub.Subscribe("id:" + igra.Id, (channel, message) =>
                {
                    _hubContext.Clients.All.SendAsync("ReceiveMessage", message.ToString());
                });
            }
            
                //id:1   1------------------- subscribe -->1
                
                //id:3   3-------------------- subscribe -->3
        }

        public bool izbrisiIgricu(string id)
        {
            IDatabase db = _redis.GetDatabase();

            if(db.HashExists("id:" + id, "naziv"))
            {
                db.KeyDelete("id:" +id);
                return true;
            }

            return false;
        }
    }
}