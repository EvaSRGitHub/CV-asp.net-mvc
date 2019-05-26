using CVApp.ViewModels;
using CVApp.ViewModels.PersonalInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVApp.Common.Services
{
    public interface IPersonalInfoService
    {
        Task SaveFormData(PersonalInfoViewModel model, string userName);

        PersonalInfoOutViewModel GetFormToEditOrDelete(string userIdentifier);

        Task EditForm(PersonalInfoViewModel model);

        Task MarkFormAsDeleted(PersonalInfoViewModel model);
    }
}
