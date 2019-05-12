using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.ViewModels
{
    public class PersonalInfoViewModel
    {
        [Required, Display(Name = "First name")]
        [RegularExpression("^(?=[A-Z][a-z])([A-Za-z]|[A-Za-z][-](?=[A-Za-z])|(?=[A-Za-z]))*$", ErrorMessage = "First name could contains only english letters and hyphen")]
        public string FirstName { get; set; }

        [Required, MaxLength(50), Display(Name = "Last name")]
        [RegularExpression("^(?=[A-Z][a-z])([A-Za-z]|[A-Za-z]['-](?=[A-Za-z])|(?=[A-Za-z]))*$", ErrorMessage = "Last name could contains only english letters, hyphen and apostrophe")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your date of birth"), Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [RegularExpression(@"^[\w!#$%&'*+\-\/=?\^_`{|}~]+(\.[\w!#$%&'*+\-\/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please enter you phone number"), Display(Name = "Phnone number")]
        public string PhoneNumber { get; set; }

        public string Summary { get; set; }

        [Required(ErrorMessage = "Please provide your current photo")]
        public string PictureFile { get; set; }

        [Required(ErrorMessage = "Please provide url to your projects"), Display(Name = "Portfolio")]
        public string ProjectsUrl { get; set; }
    }
}
