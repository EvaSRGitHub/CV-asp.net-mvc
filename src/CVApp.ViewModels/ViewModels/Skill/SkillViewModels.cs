using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.ViewModels.Skill
{
    public class SkillViewModels
    {
        public abstract class SkillBaseViewModel
        {
            [Required]
            [MinLength(3)]
            public string Name { get; set; }

            [Required]
            public int ResumeId { get; set; }
        }

        public class SkillInputViewModel : SkillBaseViewModel {}

        public class SkillEditViewModel : SkillBaseViewModel
        {
            [Required]
            public int Id { get; set; }
        }

        public class SkillOutViewModel
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
