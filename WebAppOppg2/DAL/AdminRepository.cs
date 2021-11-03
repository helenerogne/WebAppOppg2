using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{
    public class AdminRepository : IAdminRepository
    {
        private TicketDB _db;

        private ILogger<AdminRepository> _log;

        public AdminRepository(TicketDB adminDB, ILogger<AdminRepository> log)
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
                var editObject = await _db.AdminUsers.FindAsync(editAdmin.ID);
                editObject.Username = editAdmin.Username;
                //editObject.Password = editAdmin.Password;
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
                //hvis noe er feil, sjekk at adminuser reference er riktig
                AdminUser funnetAdmin = await _db.AdminUsers.FirstOrDefaultAsync(b => b.Username == admin.Username);
                //sjekk passordet
                byte[] hash = makeHash(admin.Password, funnetAdmin.Salt);
                bool ok = hash.SequenceEqual(funnetAdmin.Password);

                if (ok)
                {
                    return true;
                }
                return false;

            }

            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }

        }

        public static byte[] makeHash(string passord, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: passord,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
                                numBytesRequested: 32);
        }

        public static byte[] makeSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }

    }
}
