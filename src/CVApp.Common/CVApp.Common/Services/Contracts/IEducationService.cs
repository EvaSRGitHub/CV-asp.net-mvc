using System.Threading.Tasks;
using static CVApp.ViewModels.Education.EducationViewModels;

namespace CVApp.Common.Services
{
    public interface IEducationService
    {
        Task SaveFormData(EducationInputViewModel model, string userName);

        Task<EducationEditViewModel> EditDeleteForm(int id, string userName);

        Task Update(EducationEditViewModel model);

        //Task<EducationEditViewModel> DeleteForm(int id, string userName);

        Task Delete(EducationEditViewModel model);
    }
}
