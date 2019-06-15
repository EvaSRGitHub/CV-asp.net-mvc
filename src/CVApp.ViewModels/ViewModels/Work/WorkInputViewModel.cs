using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.ViewModels.Work
{
    public class WorkInputViewModel
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public DateTime FromYear { get; set; }

        public DateTime? ToYear { get; set; }

        [Required]
        public int ResumeId { get; set; }
    }
}
