using System;
using System.Web.Http;

namespace Assignment6
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
