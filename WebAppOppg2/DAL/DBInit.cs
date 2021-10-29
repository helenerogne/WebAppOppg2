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
                //var contextAdmin = serviceScope.ServiceProvider.GetService<AdminDB>();
                contextTicket.Database.EnsureDeleted();
                contextTicket.Database.EnsureCreated();
                //contextAdmin.Database.EnsureDeleted();
                //contextAdmin.Database.EnsureCreated();


                //PassengerTypes

                var baby = new PassengerTypes{PassengerTypeName = "baby",Discount = 100};
                var barn = new PassengerTypes{PassengerTypeName = "barn",Discount = 50};
                var voksen = new PassengerTypes{PassengerTypeName = "voksen",Discount = 0};
                var student = new PassengerTypes{PassengerTypeName = "student",Discount = 10};
                var honnør = new PassengerTypes{PassengerTypeName = "honnør",Discount = 25};

                contextTicket.PassengerTypes.Add(baby);
                contextTicket.PassengerTypes.Add(barn);
                contextTicket.PassengerTypes.Add(voksen);
                contextTicket.PassengerTypes.Add(student);
                contextTicket.PassengerTypes.Add(honnør);

                //TravelTypes

                var envei = new TravelTypes { TravelTypeName = "Envei" };
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

                var route1 = new Routes { PortFrom = port1, PortTo = port2, Departure = "10:00", RoutePrice = 100, TravelType = envei };
                var route2 = new Routes { PortFrom = port3, PortTo = port5, Departure = "17:00", RoutePrice = 249, TravelType = retur };
                var route3 = new Routes { PortFrom = port1, PortTo = port4, Departure = "10:00", RoutePrice = 149, TravelType = cruise };
                var route4 = new Routes { PortFrom = port7, PortTo = port6, Departure = "14:00", RoutePrice = 99, TravelType = envei };

                contextTicket.Routes.Add(route1);
                contextTicket.Routes.Add(route2);
                contextTicket.Routes.Add(route3);
                contextTicket.Routes.Add(route4);

                //Passengers

                var passenger1 = new Passengers{Firstname = "Per",Lastname = "Hansen",Email = "per@hansen.com", PassengerType = honnør};
                var passenger2 = new Passengers{Firstname = "Lise",Lastname = "Persen",Email = "lise@persen.com", PassengerType = barn};
                var passenger3 = new Passengers { Firstname = "Amir", Lastname = "Noor", Email = "amir@noor.com", PassengerType = student };
                var passenger4 = new Passengers { Firstname = "Bruce", Lastname = "Wayne", Email = "bat@man.com", PassengerType = voksen };

                contextTicket.Passengers.Add(passenger1);
                contextTicket.Passengers.Add(passenger2);
                contextTicket.Passengers.Add(passenger3);
                contextTicket.Passengers.Add(passenger4);

                //Tickets

                var ticket1 = new Tickets{Passenger = passenger1, Route = route1, TicketDate = "10.11.2021"};
                var ticket2 = new Tickets { Passenger = passenger4, Route = route4, TicketDate = "15.12.2021" };
                var ticket3 = new Tickets { Passenger = passenger2, Route = route2, TicketDate = "08.11.2021" };
                var ticket4 = new Tickets { Passenger = passenger3, Route = route3, TicketDate = "17.11.2022" };

                contextTicket.Tickets.Add(ticket1);
                contextTicket.Tickets.Add(ticket2);
                contextTicket.Tickets.Add(ticket3);
                contextTicket.Tickets.Add(ticket4);


                contextTicket.SaveChanges();

                /*
                //hvis noe er feil sjekk om vi ikke må lage to DBInit

                // 1. lag en påoggingsbruker
                var adminUser = new AdminUser();
                adminUser.Username = "Admin"; //2. endre til våre egne navn
                string password = "Test11"; //3. endre til våre egne navn
                byte[] salt = AdminRepository.makeSalt();
                byte[] hash = AdminRepository.makeHash(password, salt);
                adminUser.Password = hash;
                adminUser.Salt = salt;
                contextAdmin.AdminUsers.Add(adminUser);

                //4. lag en bruker til

                contextAdmin.SaveChanges();
                */
            }
        }
    }
}
