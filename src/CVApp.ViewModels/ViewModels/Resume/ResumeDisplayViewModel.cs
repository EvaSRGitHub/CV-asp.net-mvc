using System.Collections.Generic;
using static CVApp.ViewModels.Education.EducationViewModels;
using static CVApp.ViewModels.Language.LanguageViewModels;
using static CVApp.ViewModels.PersonalInfo.PersonalInfoViewModels;
using static CVApp.ViewModels.Skill.SkillViewModels;
using static CVApp.ViewModels.Work.WorkViewModels;

namespace CVApp.ViewModels.Resume
{
    public class ResumeDisplayViewModel
    { 
        public int Id { get; set; }

        public PersonalInfoOutViewModel PersonalInfo { get; set; }

        public IEnumerable<EducationOutViewModel> Educations { get; set; }

        public IEnumerable<WorkOutViewModel> Employments { get; set; }

        public IEnumerable<LanguageOutViewModel> Languages { get; set; }

        public IEnumerable<SkillOutViewModel> Skills { get; set; }
    }
}
