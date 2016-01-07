using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiHelpnGo.Models
{
    public class SimpleCity
    {

        public decimal City_id { get; set; }
        public string Name { get; set; }
        public decimal Postal_code { get; set; }
        public string Country { get; set; }

        public SimpleCity()
        {

        }

    }
}