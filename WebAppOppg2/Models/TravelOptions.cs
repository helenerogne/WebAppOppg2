using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppOppg2.Models
{
    public class TravelOptions
    {
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]")]
        public string RouteTo { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]")]
        public string RouteFrom { get; set; }
        [RegularExpression(@"[0-9]{1,1000}")]
        public int Price { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]")]
        public string Type { get; set; }
    }
}
