[assembly: HostingStartup(typeof(AdminDashboard.Web.Areas.Identity.IdentityHostingStartup))]

namespace AdminDashboard.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
