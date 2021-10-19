using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{
    public class TicketRepository : ITicketRepository
    {
        private TicketDB _db;

        private ILogger<TicketRepository> _log;

        public TicketRepository(TicketDB ticketDB, ILogger<TicketRepository> log)
        {
            _db = ticketDB;
            _log = log;
        }

        public async Task<bool> SaveTicket(Ticket ticket)
        {

            try
            {
                _db.Tickets.Add(ticket);
                await _db.SaveChangesAsync();
                return true;
            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i SaveTicket");
                return false;
            }
        }

        public async Task<List<Ticket>> GetAll()
        {
            try
            {
                List<Ticket> allTickets = await _db.Tickets.Select(t => new Ticket
                {
                    ID = t.ID,
                    Firstname = t.Firstname,
                    Lastname = t.Lastname,
                    Email = t.Email,
                    Route = t.Route,
                    Date = t.Date,
                    Quantity = t.Quantity,
                    Type = t.Type,
                    Price = t.Price
                }).ToListAsync();
                return allTickets;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetAll");
                return null;
            }
        }

        public async Task<bool> DeleteTicket(int id)
        {
            try
            {
                Tickets oneTicket = await _db.Tickets.FindAsync(id);
                _db.Tickets.Remove(oneTicket);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i DeleteTicket");
                return false;
            }
        }

        public async Task<Ticket> GetOne(int id)
        {
            try
            {
                Ticket oneTicket = await _db.Tickets.FindAsync(id);
                var gotTicket = new Ticket()
                {
                    ID = oneTicket.ID,
                    Firstname = oneTicket.Firstname,
                    Lastname = oneTicket.Lastname,
                    Email = oneTicket.Email,
                    Route = oneTicket.Route,
                    Date = oneTicket.Date,
                    Quantity = oneTicket.Quantity,
                    Type = oneTicket.Type,
                    Price = oneTicket.Price
                };
                return gotTicket;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i GetOne");
                return null;
            }
        }

        public async Task<bool> EditTicket(Ticket editTicket)
        {
            try
            {
                var editObject = await _db.Tickets.FindAsync(editTicket.ID);
                editObject.Firstname = editTicket.Firstname;
                editObject.Lastname = editTicket.Lastname;
                editObject.Email = editTicket.Email;
                editObject.Route = editTicket.Route;
                editObject.Date = editTicket.Date;
                editObject.Quantity = editTicket.Quantity;
                editObject.Type = editTicket.Type;
                editObject.Price = editTicket.Price;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i editTicket");
                return false;
            }
            return true;
        }


        public async Task<bool> EditAdmin(Admin editAdmin)
        {
            try
            {
                var editObject = await _db.Admins.FindAsync(editAdmin.Id);
                editObject.Username = editAdmin.Username;
                editObject.Password = editAdmin.Password;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i editAdmin");
                return false;
            }
            return true;
        }
    }
}
