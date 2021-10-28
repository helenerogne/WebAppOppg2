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
                var contextAdmin = serviceScope.ServiceProvider.GetService<AdminDB>();
                contextTicket.Database.EnsureDeleted();
                contextTicket.Database.EnsureCreated();
                //contextAdmin.Database.EnsureDeleted();
                //contextAdmin.Database.EnsureCreated();


                //PassengerTypes

                var baby = new PassengerTypes{PassengerTypeID = 1,PassengerTypeName = "baby",Discount = 100};
                var barn = new PassengerTypes{PassengerTypeID = 2,PassengerTypeName = "barn",Discount = 50};
                var voksen = new PassengerTypes{PassengerTypeID = 3,PassengerTypeName = "voksen",Discount = 0};
                var student = new PassengerTypes{PassengerTypeID = 4,PassengerTypeName = "student",Discount = 10};
                var honnør = new PassengerTypes{PassengerTypeID = 5,PassengerTypeName = "honnør",Discount = 25};

                contextTicket.PassengerTypes.Add(baby);
                contextTicket.PassengerTypes.Add(barn);
                contextTicket.PassengerTypes.Add(voksen);
                contextTicket.PassengerTypes.Add(student);
                contextTicket.PassengerTypes.Add(honnør);

                //TravelTypes

                var envei = new TravelTypes { TravelTypeID = 1, TravelTypeName = "Envei" };
                var retur = new TravelTypes { TravelTypeID = 2, TravelTypeName = "Tur/Retur" };
                var cruise = new TravelTypes { TravelTypeID = 3, TravelTypeName = "Cruise" };

                contextTicket.TravelTypes.Add(envei);
                contextTicket.TravelTypes.Add(retur);
                contextTicket.TravelTypes.Add(cruise);

                //Ports

                var port1 = new Ports { PortID = 1, PortName = "Bergen" };
                var port2 = new Ports { PortID = 2, PortName = "Hirtshals" };
                var port3 = new Ports { PortID = 3, PortName = "Langesund" };
                var port4 = new Ports { PortID = 4, PortName = "Kristiansand" };
                var port5 = new Ports { PortID = 5, PortName = "Stavanger" };
                var port6 = new Ports { PortID = 6, PortName = "Sandefjord" };
                var port7 = new Ports { PortID = 7, PortName = "Stromstad" };

                contextTicket.Ports.Add(port1);
                contextTicket.Ports.Add(port2);
                contextTicket.Ports.Add(port3);
                contextTicket.Ports.Add(port4);
                contextTicket.Ports.Add(port5);
                contextTicket.Ports.Add(port6);
                contextTicket.Ports.Add(port7);

                //Route

                var route1 = new Routes { RouteID = 1, PortFrom = port1, PortTo = port2, DepartureOption1 = "10:00", DepartureOption2 = "17:00", RoutePrice = 100, TravelType = envei };

                contextTicket.Routes.Add(route1);

                //Passengers

                var passenger1 = new Passengers{Firstname = "Per",Lastname = "Hansen",Email = "per@hansen.com",PassengerType = voksen};
                var passenger2 = new Passengers{Firstname = "Lise",Lastname = "Persen",Email = "lise@persen.com",PassengerType = barn};

                contextTicket.Passengers.Add(passenger1);
                contextTicket.Passengers.Add(passenger2);

                //Tickets

                var ticket1 = new Tickets{Passenger = passenger1, Route = route1, TicketDate = "10.11.2021"};

                contextTicket.Tickets.Add(ticket1);


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
