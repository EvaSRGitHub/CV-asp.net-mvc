﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.Models
{
    public class WorkExperience:BaseModel<int>
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
        public DateTime FromYear { get; set; }

        [Required]
        public DateTime ToYear { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual CVAppUser User { get; set; }
    }
}
