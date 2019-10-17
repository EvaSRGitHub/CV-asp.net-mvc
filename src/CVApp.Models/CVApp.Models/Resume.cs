using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CVApp.Models
{
    public class Resume : BaseModel<int>
    {
        public Resume()
        {
            this.Education = new HashSet<Education>();
            this.Works = new HashSet<WorkExperience>();
            this.Skills = new HashSet<Skill>();
            this.Languages = new HashSet<Language>();
        }

        public CVAppUser User { get; set; }

        [ForeignKey("CVAppUser"), Required]
        public string UserId { get; set; }

        public IEnumerable<Education> Education { get; set; }

        public IEnumerable<WorkExperience> Works { get; set; }

        public IEnumerable<Language> Languages { get; set; }

        public IEnumerable<Skill> Skills { get; set; }

    }
}
