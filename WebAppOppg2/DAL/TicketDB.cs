using WebAppOppg2.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace WebAppOppg2.DAL
{
    public class Ticket
    {
        public int TicketID { get; set; }
        public Passenger Passenger { get; set; }
        public string TicketTravelType { get; set; }
        public string TicketRoute { get; set; }
        public string TicketDeparture { get; set; }
        public string TicketDate { get; set; }
        public int TicketPrice { get; set; }
    }

    public class Port
    {
        public int PortID { get; set; }
        public string PortName { get; set; }
    }

    public class TravelType
    {
        public int TravelTypeID { get; set; }
        public string TravelTypeName { get; set; }
    }

    public class Route
    {
        public int RouteID { get; set; }
        public int TravelTypeFK { get; set; }
        public int PortFromFK { get; set; }
        public int PortToFK { get; set; }
        public int RoutePrice { get; set; }
        public string DepartureOption1 { get; set; }
        public string DepartureOption2 { get; set; }
    }

    public class Passenger
    {
        public int PassengerID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int PassengerTypeFK { get; set; }
    }

    public class PassengerType
    {
        public int PassengerTypeID { get; set; }
        public string PassengerTypeName { get; set; }
        public int Discount { get; set; }
    }


    public class TicketDB : DbContext
    {
        public TicketDB(DbContextOptions<TicketDB> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<TravelType> TravelTypes { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<PassengerType> PassengerTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // må importere pakken Microsoft.EntityFrameworkCore.Proxies
            // og legge til"viritual" på de attriuttene som ønskes å lastes automatisk (LazyLoading)
            //optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
