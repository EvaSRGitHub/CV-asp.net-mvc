using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.Models
{
    public class Language:BaseModel<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Level { get; set; }

        [Required]
        public int ResumeId { get; set; }
        public Resume Resume { get; set; }
    }
}
