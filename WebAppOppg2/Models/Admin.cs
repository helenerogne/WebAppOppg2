using System;
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
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Username { get; set; }
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{3,}$")]
        public string Password { get; set; }
    }
}
