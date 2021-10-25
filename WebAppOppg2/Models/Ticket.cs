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
        public int TicketID { get; set; }
        [ForeignKey("PassengerID")]
        public Passenger TicketPassenger { get; set; }
        public string TicketTravelType { get; set; }
        public string TicketRoute { get; set; }
        public string TicketDeparture { get; set; }
        public string TicketDate { get; set; }
        public int TicketPrice { get; set; }
    }

    public class Port
    {
        [Key]
        public int PortID { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string PortName { get; set; }
    }

    public class TravelType
    {
        [Key]
        public int TravelTypeID { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string TravelTypeName { get; set; }
    }

    public class Route
    {
        [Key]
        public int RouteID { get; set; }
        [ForeignKey("TravelType")]
        public int TravelTypeFK { get; set; }
        [ForeignKey("Port")]
        public int PortFromFK { get; set; }
        [ForeignKey("Port")]
        public int PortToFK { get; set; }
        [RegularExpression(@"[0-9]{1,100}")]
        public int RoutePrice { get; set; }
        public string DepartureOption1 { get; set; }
        public string DepartureOption2 { get; set; }
    }

    public class Passenger
    {
        [Key]
        public int PassengerID { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Firstname { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Lastname { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string Email { get; set; }
        [ForeignKey("PassengerType")]
        public int PassengerTypeFK { get; set; }
    }

    public class PassengerType
    {
        [Key]
        public int PassengerTypeID { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string PassengerTypeName { get; set; }
        [RegularExpression(@"[0-9]{1,100}")]
        public int Discount { get; set; }
    }
}
