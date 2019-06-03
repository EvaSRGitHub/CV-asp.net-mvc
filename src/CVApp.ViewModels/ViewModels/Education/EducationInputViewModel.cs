using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.ViewModels.Education
{
    public class EducationInputViewModel
    {
        [Required]
        public string Institution { get; set; }

        [Required]
        public string Diploma { get; set; }

        [Required(ErrorMessage = "The value of FromYear field is required.")]
        public DateTime FromYear { get; set; }

        [Required(ErrorMessage = "The value of ToYear field is required.")]
        public DateTime ToYear { get; set; }

        [Required]
        [RegularExpression("^[\\d]+$", ErrorMessage = "The GPA field must be number.")]
        public double GPA { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public int ResumeId { get; set; }
    }
}
