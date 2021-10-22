using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{
    public class AdminRepository : IAdminRepository
    {
        private AdminDB _db;

        private ILogger<AdminRepository> _log;

        public AdminRepository(AdminDB adminDB, ILogger<AdminRepository> log)
        {
            _db = adminDB;
            _log = log;
        }

        public async Task<bool> EditAdmin(Admin editAdmin)
        {
            /*string x = "somestring";
            byte[] y = System.Text.Encoding.UTF8.GetBytes(x);
            */

            try
            {
                var editObject = await _db.Admins.FindAsync(editAdmin.ID);
                editObject.Username = editAdmin.Username;
               // editObject.Password = editAdmin.Password;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message + "Feil i editAdmin");
                return false;
            }
            return true;
        }
        public async Task<bool> LogIn(Admin admin)
        {
            try
            {
                Admins funnetAdmin = await _db.Admins.FirstOrDefaultAsync(b => b.Username == admin.Username);
                return true;
            }
            /* sjekk passordet
            byte[] hash = LagHash(bruker.Passord, funnetBruker.Salt);
            bool ok = hash.SequenceEqual(funnetBruker.Passord);
            if (ok)
            {
                return true;
            }
            return false;
        }
            */
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }

        }
               
    }
}
