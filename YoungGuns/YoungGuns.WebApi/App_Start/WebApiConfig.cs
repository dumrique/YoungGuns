using System.Web.Http;
using System.Web.Http.Cors;

namespace YoungGuns.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
      {
            config.MapHttpAttributeRoutes();

            var cors = new EnableCorsAttribute(origins: "*", headers: "*", methods: "*");
            config.EnableCors(cors);
        }
    }
}
