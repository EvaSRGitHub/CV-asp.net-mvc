using System.Threading.Tasks;
using static CVApp.ViewModels.Language.LanguageViewModels;

namespace CVApp.Common.Services
{
    public interface ILanguageService
    {
        Task SaveFormData(LanguageInputViewModel model, string userName);

        Task<LanguageEditViewModel> EditDeleteForm(int id, string userName);

        Task Update(LanguageEditViewModel model);

        Task Delete(LanguageEditViewModel model);
    }
}
