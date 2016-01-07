using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpnGo.Model
{
    public class City
    {
        public decimal City_id { get; set; }
        public string Name { get; set; }
        public decimal Postal_code { get; set; }
        public string Country { get; set; }
        public int Postal_code_to_display { get; set; }

        public City(decimal city_id, string name, decimal postal_code, string country)
        {
            City_id = city_id;
            Name = name;
            Postal_code = postal_code;
            Country = country;
        }

        public City()
        {

        }

    }
}
