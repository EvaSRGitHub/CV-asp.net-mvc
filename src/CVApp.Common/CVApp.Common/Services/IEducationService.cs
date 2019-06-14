using CVApp.ViewModels.Education;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVApp.Common.Services
{
    public interface IEducationService
    {
        Task SaveFormData(EducationInputViewModel model, string userName);

        Task<EducationEditViewModel> EditForm(int id);

        Task Update(EducationEditViewModel model);

        Task<EducationEditViewModel> DeleteForm(int id);

        Task Delete(EducationEditViewModel model);
    }
}
