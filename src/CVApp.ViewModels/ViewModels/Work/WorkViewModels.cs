using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.ViewModels.Work
{
    public class WorkViewModels
    {
        public abstract class WorkBaseViewModel : IValidatableObject
        {
            [Required]
            [Display(Name = "Employer")]
            public string Company { get; set; }

            [Required]
            [Display(Name = "Job Title")]
            public string Title { get; set; }

            [Required]
            [Display(Name = "Job Description")]
            public string Description { get; set; }

            [Required]
            public string Country { get; set; }

            [Required]
            public string Region { get; set; }

            [Required]
            public string City { get; set; }

            [Required]
            [Display(Name = "Start Date")]
            public DateTime StartDate { get; set; }

            [Display(Name = "End Date")]
            public DateTime? EndDate { get; set; }

            [Required]
            public int ResumeId { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (this.EndDate.HasValue && this.EndDate.Value < this.StartDate)
                {
                    yield return new ValidationResult($"{nameof(this.EndDate)} can't be before start date.", new List<string>() { $"{nameof(this.EndDate)}" });
                }

                if (this.StartDate > DateTime.Now)
                {
                    yield return new ValidationResult($"{nameof(this.StartDate)} can't be in the future.", new List<string>() { $"{nameof(this.StartDate)}" });
                }

                if (this.EndDate.HasValue && this.EndDate.Value > DateTime.Now)
                {
                    yield return new ValidationResult($"{nameof(this.EndDate)} can't be in the future.", new List<string>() { $"{nameof(this.EndDate)}" });
                }
            }
        }

        public class WorkInputViewModel : WorkBaseViewModel { }

        public class WorkEditViewModel : WorkBaseViewModel 
        {
            public int Id { get; set; }

        }

        public class WorkOutViewModel
        {
            public int Id { get; set; }

            public string Company { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }

            public string Country { get; set; }

            public string City { get; set; }

            public string StartDate { get; set; }

            public string EndDate { get; set; }
        }
    }
}
