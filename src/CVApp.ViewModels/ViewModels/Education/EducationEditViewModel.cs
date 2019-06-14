using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.ViewModels.Education
{
    public class EducationEditViewModel
    {
        [Required]
        public int Id { get; set; }

        public EducationInputViewModel InputVM { get; set; }
    }
}
