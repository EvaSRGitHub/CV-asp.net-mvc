using CVApp.ViewModels.Resume;
using System.Threading.Tasks;

namespace CVApp.Common.Services.Contracts
{
    public interface IResumeService
    {
        Task CreateResume(string userId);

        Task<ResumeDisplayViewModel> DisplayResume(string userName);

        Task Delete(int id, string userName);
    }
}
