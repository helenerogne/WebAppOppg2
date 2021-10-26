﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppOppg2.DAL
{
    public class AdminUser
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public byte[] Password { get; set; }
            public byte[] Salt { get; set; }
        }
        public class AdminDB : DbContext
        {
            public AdminDB(DbContextOptions<AdminDB> options)
                : base(options)
            {
                Database.EnsureCreated();
            }
            public DbSet<AdminUser> AdminUsers { get; set; }
        }
}
