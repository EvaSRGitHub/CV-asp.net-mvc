using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.ViewModels.Skill
{
    public class SkillInputViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int ResumeId { get; set; }
    }
}
