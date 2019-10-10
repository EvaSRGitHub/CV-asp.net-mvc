using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CVApp.ViewModels.Language
{
    public class LanguageViewModels
    {
        public abstract class LanguageBaseViewModel : IValidatableObject
        {
            [Required]
            [MinLength(3)]
            public string Name { get; set; }

            [Required]
            public string Level { get; set; }

            [Required]
            public int ResumeId { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (string.IsNullOrWhiteSpace(this.Name))
                {
                    yield return new ValidationResult($"Field {nameof(this.Name)} can't be empty.", new List<string> { nameof(this.Name) });
                }
            }
        }

        public class LanguageInputViewModel : LanguageBaseViewModel { }

        public class LanguageEditViewModel : LanguageBaseViewModel
        {
            [Required]
            public int Id { get; set; }
        }

        public class LanguageOutViewModel
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Level { get; set; }
        }
    }
}
