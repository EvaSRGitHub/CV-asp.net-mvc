using CVApp.ViewModels.Resume;
using System.Threading.Tasks;

namespace CVApp.Common.Services.Contracts
{
    public interface IResumeService
    {
        Task CreateResume(string userId);

        ResumeDisplayViewModel DisplayResume();

        Task Delete(int id);

        ResumeStartViewModel GetStartInfo();
    }
}
