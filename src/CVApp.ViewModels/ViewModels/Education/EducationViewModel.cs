using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;

namespace CVApp.ViewModels.Education
{
    public class EducationViewModels
    {
        public abstract class EducationBaseViewModel : IValidatableObject
        {
            [Required]
            [MinLength(3)]
            [Display(Name = "Institution Name")]
            public string Institution { get; set; }

            [Required]
            [MinLength(3)]
            [Display(Name = "Diploma Title")]
            public string Diploma { get; set; }

            [Required]
            [Display(Name = "Start Date")]
            public DateTime StartDate { get; set; }

            [Display(Name = "End Date")]
            public DateTime? EndDate { get; set; }

            [Required]
            [Range(2.0, 6.0)]
            [RegularExpression(@"^[\d.]+$", ErrorMessage = "The GPA field must be a number.")]
            public double GPA { get; set; }

            [Required]
            [MinLength(10)]
            [Display(Name = "Main Subjects")]
            public string MainSubjects { get; set; }

            [Required]
            public string City { get; set; }

            [Required]
            public string Country { get; set; }

            [Required]
            public string Region { get; set; }

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

                if (string.IsNullOrWhiteSpace(Regex.Replace(HttpUtility.HtmlDecode(this.MainSubjects), @"<[^>]*>", "")))
                {
                    yield return new ValidationResult($"{nameof(this.MainSubjects)} can't be empty.", new List<string>() { $"{nameof(this.MainSubjects)}" });
                }
            }
        }
         
        public class EducationInputViewModel : EducationBaseViewModel
        {
        }

        public class EducationEditViewModel : EducationBaseViewModel
        {
            [Required]
            public int Id { get; set; }
        }

        public class EducationOutViewModel
        {
            [Required]
            public string Institution { get; set; }

            [Required]
            public string Diploma { get; set; }

            [Required]
            public string StartDate { get; set; }

            [Required]
            public string EndDate { get; set; }

            [Required]
            public double GPA { get; set; }

            [Required]
            public string MainSubjects { get; set; }

            [Required]
            public string City { get; set; }

            [Required]
            public string Country { get; set; }

            [Required]
            public int EducationId { get; set; }
        }
    }
   
}
