using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StickyNotes.Startup))]
namespace StickyNotes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
