using WebAppOppg2.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace WebAppOppg2.DAL
{
    public class Ticket
    {
        public int ID { get; set; }
        public Passenger Passenger { get; set; }
        public Route Route { get; set; }
        public string Date { get; set; }

    }

    public class Port
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class TravelType
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Route
    {
        public int ID { get; set; }
        public TravelType TravelType { get; set; }
        public Port PortFrom { get; set; }
        public Port PortTo { get; set; }
        public int Price { get; set; }
        public string DepartureOption1 { get; set; }
        public string DepartureOption2 { get; set; }
    }

    public class Passenger
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public PassengerType PassengerType { get; set; }
    }

    public class PassengerType
    {
        public int ID { get; set; }
        public string Name { get; set; }
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
    }
}
