using System.Threading.Tasks;

namespace CVApp.Common.GeneratePDF.Contracts
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
