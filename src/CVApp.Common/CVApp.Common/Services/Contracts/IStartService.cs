using CVApp.ViewModels.Start;
using System.Threading.Tasks;

namespace CVApp.Common.Services.Contracts
{
    public interface IStartService
    {
        Task<StartOutViewModel> GetStartInfoByUserName(string userName);
    }
}
