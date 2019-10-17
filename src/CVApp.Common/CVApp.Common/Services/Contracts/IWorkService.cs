using System.Collections.Generic;
using System.Threading.Tasks;
using static CVApp.ViewModels.Work.WorkViewModels;

namespace CVApp.Common.Services.Contracts
{
    public interface IWorkService
    {
        Task<int> SaveFormData(WorkInputViewModel model, string userName);

        Task<WorkEditViewModel> EditDeleteForm(int id, string userName);

        Task Update(WorkEditViewModel model);

        Task Delete(WorkEditViewModel model);

        Task<IEnumerable<WorkOutViewModel>> GetWorkInfo(int resumeId);
    }
}
