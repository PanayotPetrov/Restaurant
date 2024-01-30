namespace Restaurant.Web.HelperClasses
{
    using System.Threading.Tasks;

    public interface IViewHtmlRenderer
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
