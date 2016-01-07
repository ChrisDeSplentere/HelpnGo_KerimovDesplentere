using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiHelpnGo.Models
{
    public class SimplePublication
    {
        public decimal Publication_id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Phone_number { get; set; }
        public string Email { get; set; }
        public decimal Author_id { get; set; }
        public string Category_label { get; set; }
        public string Province_label { get; set; }
        public bool Is_offer { get; set; }

        public SimplePublication()
        {

        }
    }
}