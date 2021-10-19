using WebAppOppg2.Models;

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
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Route { get; set; }
        public string Date { get; set; }
        public int Count { get; set; }
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
        public DbSet<Admin> Admins { get; set; }
    }
}
