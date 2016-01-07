using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiHelpnGo.Models
{
    public class SimpleUser
    {
        public decimal User_id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Nullable<System.DateTime> Date_of_birth { get; set; }
        public string Street_name { get; set; }
        public string Street_number { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public string Phone_number { get; set; }
        public string Diplomas { get; set; }
        public string Jobs { get; set; }
        public decimal Living_city_id { get; set; }

        public SimpleUser()
        {

        }
    }
}