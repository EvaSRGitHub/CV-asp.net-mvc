using System.Collections.Generic;
using System.Threading.Tasks;
using static CVApp.ViewModels.Language.LanguageViewModels;

namespace CVApp.Common.Services.Contracts
{
    public interface ILanguageService
    {
        Task<int> SaveFormData(LanguageInputViewModel model, string userName);

        Task<LanguageEditViewModel> EditDeleteForm(int id, string userName);

        Task Update(LanguageEditViewModel model);

        Task Delete(LanguageEditViewModel model);

        Task<IEnumerable<LanguageOutViewModel>> GetLanguageInfo(int resumeId);
    }
}
