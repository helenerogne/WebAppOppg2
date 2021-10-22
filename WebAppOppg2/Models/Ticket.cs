using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppOppg2.Models
{
    public class Ticket
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Passenger")]
        public Passenger Passenger { get; set; }
        [ForeignKey("Route")]
        public Route Route { get; set; }
        public string Date { get; set; }
    }

    public class Port
    {
        [Key]
        public int ID { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Name { get; set; }
    }

    public class TravelType
    {
        [Key]
        public int ID { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Name { get; set; }
    }

    public class Route
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("TravelType")]
        public TravelType TravelType { get; set; }
        [ForeignKey("Port")]
        public Port PortFrom { get; set; }
        [ForeignKey("Port")]
        public Port PortTo { get; set; }
        [RegularExpression(@"[0-9]{1,100}")]
        public int Price { get; set; }
        public string DepartureOption1 { get; set; }
        public string DepartureOption2 { get; set; }
    }

    public class Passenger
    {
        [Key]
        public int ID { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Firstname { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Lastname { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string Email { get; set; }
        [ForeignKey("PassengerType")]
        public PassengerType PassengerType { get; set; }
    }

    public class PassengerType
    {
        [Key]
        public int ID { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Name { get; set; }
        [RegularExpression(@"[0-9]{1,100}")]
        public int Discount { get; set; }
    }
}
