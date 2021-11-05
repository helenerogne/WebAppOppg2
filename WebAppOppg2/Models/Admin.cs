﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppOppg2.Models
{
    public class Admin
    {
        [Key]
        public int ID { get; set; }
       
        public string Username { get; set; }
       
        public string Password { get; set; }
    }
}
