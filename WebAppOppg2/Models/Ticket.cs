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
        public int PassengerID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int PassengerTypeID {get; set;}
        public string PassengerType { get; set; }
        public int RouteID { get; set; }
        public int TravelTypeID { get; set; }
        public string TravelType { get; set; }
        public string RouteFrom { get; set; }
        public string RouteTo { get; set; }
        public string Departure { get; set; }
        public string TicketDate { get; set; }
        public int Price { get; set; }
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
        public string PortFrom { get; set; }
        public string PortTo { get; set; }
        [RegularExpression(@"[0-9]{1,100}")]
        public int RoutePrice { get; set; }
        public string Departure { get; set; }
    }

    public class Passenger
    {
        [Key]
        public int PassengerID { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Firstname { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Lastname { get; set; }
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,30}")]
        public string Email { get; set; }
        public int PassengerTypeID { get; set; }
        public string PassengerType {get; set;}
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
