using WebAppOppg2.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAppOppg2.DAL
{
    public class Tickets
    {
        [Key]
        public int TicketID { get; set; }
        public int PassengerID { get; set; }
        virtual public Passengers Passenger { get; set; }
        public int RouteID { get; set; }
        virtual public Routes Route { get; set; }
        public string TicketDate { get; set; }
    }

    public class Ports
    {
        [Key]
        public int PortID { get; set; }
        public string PortName { get; set; }
    }

    public class TravelTypes
    {
        [Key]
        public int TravelTypeID { get; set; }
        public string TravelTypeName { get; set; }
    }

    public class Routes
    {
        [Key]
        public int RouteID { get; set; }
        public int TravelTypeID {get; set;}
        virtual public TravelTypes TravelType { get; set; }
        virtual public Ports PortFrom { get; set; }
        virtual public Ports PortTo { get; set; }
        public int RoutePrice { get; set; }
        public string Departure { get; set; }
    }

    public class Passengers
    {
        [Key]
        public int PassengerID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int PassengerTypeID {get; set;}
        public virtual PassengerTypes PassengerType {get; set;}
    }

    public class PassengerTypes
    {
        [Key]
        public int PassengerTypeID { get; set; }
        public string PassengerTypeName { get; set; }
        public int Discount { get; set; }
    }

    public class AdminUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
    }


    public class TicketDB : DbContext
    {
        public TicketDB(DbContextOptions<TicketDB> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<Ports> Ports { get; set; }
        public DbSet<TravelTypes> TravelTypes { get; set; }
        public DbSet<Routes> Routes { get; set; }
        public DbSet<Passengers> Passengers { get; set; }
        public DbSet<PassengerTypes> PassengerTypes { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // må importere pakken Microsoft.EntityFrameworkCore.Proxies
            // og legge til"viritual" på de attriuttene som ønskes å lastes automatisk (LazyLoading)
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
