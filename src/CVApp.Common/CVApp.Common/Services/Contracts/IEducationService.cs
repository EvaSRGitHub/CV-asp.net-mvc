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

        Task<EducationEditViewModel> EditForm(int id, string userName);

        Task Update(EducationEditViewModel model);

        Task<EducationEditViewModel> DeleteForm(int id, string userName);

        Task Delete(EducationEditViewModel model);
    }
}
