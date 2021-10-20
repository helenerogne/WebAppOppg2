using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{
    public interface ITicketRepository
    {
        Task<bool> SaveTicket(Ticket ticket);
        Task<bool> DeleteTicket(int id);
        Task<bool> EditTicket(Ticket editTicket);
        Task<Ticket> GetOne(int id);
        Task<List<Ticket>> GetAll();
        Task<bool> EditAdmin(Admin editAdmin);
    }
}