using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TopPuzzle.ru.Startup))]
namespace TopPuzzle.ru
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
