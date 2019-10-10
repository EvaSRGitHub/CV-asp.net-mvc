using System.Threading.Tasks;
using static CVApp.ViewModels.PersonalInfo.PersonalInfoViewModels;

namespace CVApp.Common.Services
{
    public interface IPersonalInfoService
    {
        Task SaveFormData(PersonalInfoBaseViewModel model, string userName);

        Task<PersonalInfoEditViewModel> EditForm(string userName);

        Task DeletePicture(string userName);

        Task<bool> HasPersonalInfoFormFilled(string userName);
    }
}
