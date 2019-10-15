using System.Threading.Tasks;
using static CVApp.ViewModels.Skill.SkillViewModels;

namespace CVApp.Common.Services.Contracts
{
    public interface ISkillService
    {
        Task<int> SaveFormData(SkillInputViewModel model, string userName);

        Task<SkillEditViewModel> EditDeleteForm(int id, string userName);

        Task Update(SkillEditViewModel model);

        Task Delete(SkillEditViewModel model);
    }
}
