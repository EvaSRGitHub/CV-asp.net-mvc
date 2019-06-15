using CVApp.ViewModels.Education;
using CVApp.ViewModels.PersonalInfo;
using CVApp.ViewModels.Work;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVApp.ViewModels.Resume
{
    public class ResumeDisplayViewModel
    {
        public PersonalInfoOutViewModel PersonalInfo { get; set; }

        public IEnumerable<EducationOutViewModel> Educations { get; set; }

        public IEnumerable<WorkOutViewModel> Employments { get; set; }
    }
}
