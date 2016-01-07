using System;
using System.Collections.Generic;
using System.Text;

namespace HelpnGo.Model
{
    public class Category
    {
        public string Label { get; set; }

        public Category(string label)
        {
            Label = label;
        }

        public Category()
        {

        }
    }
}
