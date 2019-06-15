using System;
using System.Collections.Generic;
using System.Text;

namespace CVApp.ViewModels.Work
{
    public class WorkOutViewModel
    {
        public int Id { get; set; }

        public string Company { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string FromYear { get; set; }

        public string ToYear { get; set; }
    }
}
