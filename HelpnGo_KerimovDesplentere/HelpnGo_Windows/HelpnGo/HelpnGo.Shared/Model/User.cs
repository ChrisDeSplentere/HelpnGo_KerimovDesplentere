using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpnGo.Model
{
    public class User
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

        public User(decimal userID, string firstname, string lastname, DateTime dateOfBirth, string addrNum, string addrStreet, decimal locality, string email, string pwd, string phone, string diplomas, string jobs)
        {
            User_id = userID;
            Firstname = firstname;
            Lastname = lastname;
            Date_of_birth = dateOfBirth;
            Street_number = addrNum;
            Street_name = addrStreet;
            Living_city_id = locality;
            Email = email;
            password = pwd;
            Phone_number = phone;
            Diplomas = diplomas;
            Jobs = jobs;
        }

        public User()
        {

        }


    }
}
