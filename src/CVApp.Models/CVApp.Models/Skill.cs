using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.Models
{
    public class Skill:BaseModel<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual CVAppUser User { get; set; }
    }
}
