using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.ViewModels.Education
{
    public class EducationOutViewModel
    {
        [Required]
        public string Institution { get; set; }

        [Required]
        public string Diploma { get; set; }

        [Required]
        public string FromYear { get; set; }

        [Required]
        public string ToYear { get; set; }

        [Required]
        public double GPA { get; set; }

        [Required]
        public string MainSubjects { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public int EducationId { get; set; }
    }
}
