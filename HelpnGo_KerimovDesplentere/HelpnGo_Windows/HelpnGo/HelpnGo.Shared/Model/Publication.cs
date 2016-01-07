using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpnGo.Model
{
    public class Publication
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
        public string Type { get; set; }

        public Publication(decimal publication_id, string title, string description, bool offer, string phone_number, string email, string province, string category, decimal author_id)
        {
            Publication_id = publication_id;
            Title = title;
            Description = description;
            Is_offer = offer;
            Phone_number = phone_number;
            Email = email;
            Province_label = province;
            Category_label = category;
            Author_id = author_id;
        }

        public Publication()
        {

        }

    }
}
