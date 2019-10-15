using System.Threading.Tasks;

namespace CVApp.Common.IronPdfConverter
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
