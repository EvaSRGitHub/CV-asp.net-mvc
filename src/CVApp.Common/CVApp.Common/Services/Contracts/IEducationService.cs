using System.Collections.Generic;
using System.Threading.Tasks;
using static CVApp.ViewModels.Education.EducationViewModels;

namespace CVApp.Common.Services.Contracts
{
    public interface IEducationService
    {
        Task<int> SaveFormData(EducationInputViewModel model, string userName);

        Task<EducationEditViewModel> EditDeleteForm(int id, string userName);

        Task Update(EducationEditViewModel model);

        Task Delete(EducationEditViewModel model);

        Task<IEnumerable<EducationOutViewModel>> GetEducationInfo(int resumeId);
    }
}
