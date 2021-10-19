using WebAppOppg2.Models;
using System;

namespace WebAppOppg2.DAL
{

    public class Admin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Tickets
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

        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
