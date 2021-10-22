using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppOppg2.DAL
{
    public class DBInit
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var contextTicket = serviceScope.ServiceProvider.GetService<TicketDB>();
                var contextAdmin = serviceScope.ServiceProvider.GetService<AdminDB>();
                contextTicket.Database.EnsureDeleted();
                contextTicket.Database.EnsureCreated();
                contextAdmin.Database.EnsureDeleted();
                contextAdmin.Database.EnsureCreated();


                contextTicket.SaveChanges();
                
                //hvis noe er feil sjekk om vi ikke må lage to DBInit

                // 1. lag en påoggingsbruker
                var adminUser = new AdminUser();
                adminUser.Username = "Admin"; //2. endre til våre egne navn
                string password = "Test11"; //3. endre til våre egne navn
                byte[] salt = AdminRepository.makeSalt();
                byte[] hash = AdminRepository.makeHash(password, salt);
                adminUser.Password = hash;
                adminUser.Salt = salt;
                contextAdmin.AdminUsers.Add(adminUser);

                //4. lag en bruker til

                contextAdmin.SaveChanges();
            }
        }
    }
}
