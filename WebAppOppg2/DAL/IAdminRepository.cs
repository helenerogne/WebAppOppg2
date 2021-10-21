using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{
    public interface IAdminRepository
    {
        Task<bool> EditAdmin(Admin editAdmin);
        Task<bool> LogIn(Admin admin);
        //Task<bool> Loggut();
    }
}
