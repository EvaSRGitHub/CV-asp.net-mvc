using System.Threading.Tasks;
using static CVApp.ViewModels.Work.WorkViewModels;

namespace CVApp.Common.Services
{
    public interface IWorkService
    {
        Task SaveFormData(WorkInputViewModel model, string userName);

        Task<WorkEditViewModel> EditDeleteForm(int id, string userName);

        Task Update(WorkEditViewModel model);

        Task Delete(WorkEditViewModel model);
    }
}
