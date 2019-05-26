using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public string RepoProfile { get; set; }

        public Resume Resume { get; set; }

        [ForeignKey("Resume")]
        public int? ResumeId { get; set; }
    }
}
