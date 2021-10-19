using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppOppg2.Models
{
    public class Ticket
    {
        [Key]
        public int ID { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Firstname { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Lastname { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string Email { get; set; }
        public string Route { get; set; }
        public string Date { get; set; }
        public int Quantity { get; set; }
        [RegularExpression(@"[0-9]{1,100}")]
        public string Type { get; set; }
        public string Price { get; set; }
    }
}
