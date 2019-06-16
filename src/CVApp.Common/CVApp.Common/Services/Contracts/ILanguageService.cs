using CVApp.ViewModels.Language;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVApp.Common.Services
{
    public interface ILanguageService
    {
        Task SaveFormData(LanguageInputViewModel model, string userName);

        Task<LanguageEditViewModel> EditForm(int id, string userName);

        Task Update(LanguageEditViewModel model);

        Task<LanguageEditViewModel> DeleteForm(int id, string userName);

        Task Delete(LanguageEditViewModel model);
    }
}
