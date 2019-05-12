using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CVApp.Models
{
    // Add profile data for application users by adding properties to the CVAppUser class
    public class CVAppUser : IdentityUser
    {
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Address { get; set; }

        public string Summary { get; set; }

        public string Picture { get; set; }

        public string GitHubProfile { get; set; }

        public virtual IEnumerable<Education> Education { get; set; }

        public virtual IEnumerable<WorkExperience> Works { get; set; }

        public virtual IEnumerable<Certificate> Certificates { get; set; }

        public virtual IEnumerable<Language> Languages { get; set; }

        public virtual IEnumerable<Skill> Skills { get; set; }
    }
}
