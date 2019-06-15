using CVApp.ViewModels.Work;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVApp.Common.Services
{
    public interface IWorkService
    {
        Task SaveFormData(WorkInputViewModel model, string userName);

        Task<WorkEditViewModel> EditForm(int id, string userName);

        Task Update(WorkEditViewModel model);

        Task<WorkEditViewModel> DeleteForm(int id, string userName);

        Task Delete(WorkEditViewModel model);
    }
}
