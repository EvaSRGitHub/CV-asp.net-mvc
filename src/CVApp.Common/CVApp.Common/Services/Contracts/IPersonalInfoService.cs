using System.Threading.Tasks;
using static CVApp.ViewModels.PersonalInfo.PersonalInfoViewModels;

namespace CVApp.Common.Services.Contracts
{
    public interface IPersonalInfoService
    {
        Task SaveFormData(PersonalInfoBaseViewModel model);

        Task<PersonalInfoEditViewModel> EditForm(int resumeId);

        Task DeletePicture();

        Task<bool> HasPersonalInfoFormFilled();

        Task<PersonalInfoOutViewModel> GetPersonalInfo(int resumeId);
    }
}
