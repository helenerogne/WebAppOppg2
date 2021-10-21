using WebAppOppg2.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace WebAppOppg2.DAL
{
    public class Ticket
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Route { get; set; }
        public string Date { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Price { get; set; }
    }


    public class TicketDB : DbContext
    {
        public TicketDB(DbContextOptions<TicketDB> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Ticket> Tickets { get; set; }
    }
}
