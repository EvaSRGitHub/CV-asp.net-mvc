using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.Models
{
    public class Education:BaseModel<int>
    {
        [Required]
        public string Institution { get; set; }

        [Required]
        public string Diploma { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public double GPA { get; set; }

        [Required]
        public string MainSubjects { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public int ResumeId { get; set; }

        public Resume Resume { get; set; }
    }
}
