using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppOppg2.DAL
{
    public class DBInit
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var contextTicket = serviceScope.ServiceProvider.GetService<TicketDB>();
               // var contextAdmin = serviceScope.ServiceProvider.GetService<AdminDB>();
                contextTicket.Database.EnsureDeleted();
                contextTicket.Database.EnsureCreated();
                //contextAdmin.Database.EnsureDeleted();
                //contextAdmin.Database.EnsureCreated();


                //PassengerTypes

                var baby = new PassengerTypes{PassengerTypeName = "Baby",Discount = 100};
                var barn = new PassengerTypes{PassengerTypeName = "Barn",Discount = 50};
                var voksen = new PassengerTypes{PassengerTypeName = "Voksen",Discount = 0};
                var student = new PassengerTypes{PassengerTypeName = "Student",Discount = 10};
                var honnør = new PassengerTypes{PassengerTypeName = "Honnør",Discount = 25};

                contextTicket.PassengerTypes.Add(baby);
                contextTicket.PassengerTypes.Add(barn);
                contextTicket.PassengerTypes.Add(voksen);
                contextTicket.PassengerTypes.Add(student);
                contextTicket.PassengerTypes.Add(honnør);

                //TravelTypes

                var envei = new TravelTypes { TravelTypeName = "Enveis" };
                var retur = new TravelTypes { TravelTypeName = "Tur/Retur" };
                var cruise = new TravelTypes { TravelTypeName = "Cruise" };

                contextTicket.TravelTypes.Add(envei);
                contextTicket.TravelTypes.Add(retur);
                contextTicket.TravelTypes.Add(cruise);

                //Ports

                var port1 = new Ports { PortName = "Bergen" };
                var port2 = new Ports { PortName = "Hirtshals" };
                var port3 = new Ports { PortName = "Langesund" };
                var port4 = new Ports { PortName = "Kristiansand" };
                var port5 = new Ports { PortName = "Stavanger" };
                var port6 = new Ports { PortName = "Sandefjord" };
                var port7 = new Ports { PortName = "Stromstad" };

                contextTicket.Ports.Add(port1);
                contextTicket.Ports.Add(port2);
                contextTicket.Ports.Add(port3);
                contextTicket.Ports.Add(port4);
                contextTicket.Ports.Add(port5);
                contextTicket.Ports.Add(port6);
                contextTicket.Ports.Add(port7);

                //Route

                var route1 = new Routes { PortFrom = port1, PortTo = port2, Departure = "10:00", RoutePrice = 100};
                var route2 = new Routes { PortFrom = port3, PortTo = port5, Departure = "17:20", RoutePrice = 249};
                var route3 = new Routes { PortFrom = port1, PortTo = port4, Departure = "10:00", RoutePrice = 149};
                var route4 = new Routes { PortFrom = port7, PortTo = port6, Departure = "14:15", RoutePrice = 199};
                var route5 = new Routes { PortFrom = port1, PortTo = port7, Departure = "12:00", RoutePrice = 300};
                var route6 = new Routes { PortFrom = port3, PortTo = port5, Departure = "18:00", RoutePrice = 249};
                var route7 = new Routes { PortFrom = port4, PortTo = port1, Departure = "09:00", RoutePrice = 149};
                var route8 = new Routes { PortFrom = port7, PortTo = port6, Departure = "08:30", RoutePrice = 499};

                contextTicket.Routes.Add(route1);
                contextTicket.Routes.Add(route2);
                contextTicket.Routes.Add(route3);
                contextTicket.Routes.Add(route4);
                contextTicket.Routes.Add(route5);
                contextTicket.Routes.Add(route6);
                contextTicket.Routes.Add(route7);
                contextTicket.Routes.Add(route8);

                //Passengers

                var passenger1 = new Passengers{Firstname = "Per",Lastname = "Hansen",Email = "per@hansen.com", PassengerTypeID = 1};
                var passenger2 = new Passengers{Firstname = "Lise",Lastname = "Persen",Email = "lise@persen.com", PassengerTypeID = 2};
                var passenger3 = new Passengers { Firstname = "Amir", Lastname = "Noor", Email = "amir@noor.com", PassengerTypeID = 3 };
                var passenger4 = new Passengers { Firstname = "Bruce", Lastname = "Wayne", Email = "bat@man.com", PassengerTypeID = 4 };
                var passenger5 = new Passengers { Firstname = "Kari", Lastname = "Tråd", Email = "kari@tråd.com", PassengerTypeID = 2 };
                var passenger6 = new Passengers { Firstname = "Peter", Lastname = "Parker", Email = "spider@man.com", PassengerTypeID = 3 };

                contextTicket.Passengers.Add(passenger1);
                contextTicket.Passengers.Add(passenger2);
                contextTicket.Passengers.Add(passenger3);
                contextTicket.Passengers.Add(passenger4);
                contextTicket.Passengers.Add(passenger5);
                contextTicket.Passengers.Add(passenger6);

                //Tickets

                var ticket1 = new Tickets{Passenger = passenger1, Route = route1, TravelType = envei, TicketDate = "10.11.2021"};
                var ticket2 = new Tickets { Passenger = passenger4, Route = route4, TravelType = retur,TicketDate = "15.12.2021" };
                var ticket3 = new Tickets { Passenger = passenger2, Route = route2, TravelType = envei,TicketDate = "08.11.2021" };
                var ticket4 = new Tickets { Passenger = passenger3, Route = route3, TravelType = retur,TicketDate = "17.11.2022" };
                var ticket5 = new Tickets{Passenger = passenger1, Route = route7, TravelType = cruise,TicketDate = "10.03.2022"};
                var ticket6 = new Tickets { Passenger = passenger5, Route = route1, TravelType = envei,TicketDate = "18.07.2021" };
                var ticket7 = new Tickets { Passenger = passenger6, Route = route5, TravelType = cruise,TicketDate = "08.11.2021" };
                var ticket8 = new Tickets { Passenger = passenger4, Route = route8, TravelType = retur,TicketDate = "17.11.2022" };

                contextTicket.Tickets.Add(ticket1);
                contextTicket.Tickets.Add(ticket2);
                contextTicket.Tickets.Add(ticket3);
                contextTicket.Tickets.Add(ticket4);
                contextTicket.Tickets.Add(ticket5);
                contextTicket.Tickets.Add(ticket6);
                contextTicket.Tickets.Add(ticket7);
                contextTicket.Tickets.Add(ticket8);


                contextTicket.SaveChanges();

                /*
                
                - Forslag til endring av kodebiten for opprettelse av bruker (under kommentaren).
                  Blir mer ulikt Tor sin kode + matcher bedre med resten av koden ovenfor.
                - OBS! Må sjekke at den kjører ordentlig med det først i såfall!

                byte[] salt = TicketRepository.makeSalt();
                var adminUser = new AdminUser { Id = 1, Username = "Admin", Password = TicketRepository.makeHash("Admin", salt), Salt = salt };
                contextTicket.AdminUsers.Add(adminUser);
                contextTicket.SaveChanges();

                */

                var adminUser = new AdminUser();
                adminUser.Username = "Admin"; 
                string password = "Admin"; 
                byte[] salt = TicketRepository.makeSalt();
                byte[] hash = TicketRepository.makeHash(password, salt);
                adminUser.Password = hash;
                adminUser.Salt = salt;
                contextTicket.AdminUsers.Add(adminUser);

                contextTicket.SaveChanges();
                
            }
        }
    }
}
