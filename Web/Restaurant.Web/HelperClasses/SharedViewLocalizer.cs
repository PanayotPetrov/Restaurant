namespace Restaurant.Web.HelperClasses
{
    using System.Reflection;

    using Microsoft.Extensions.Localization;

    public class SharedViewLocalizer : ISharedViewLocalizer
    {
        private readonly IStringLocalizer localizer;

        public SharedViewLocalizer(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            this.localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public LocalizedString this[string key] => this.localizer[key];

        public LocalizedString GetLocalizedString(string key)
        {
            return this.localizer[key];
        }
    }
}
