using Microsoft.AspNetCore.Builder;

namespace Apex.Websites
{
    public static class StartupMvc
    {
        public static IApplicationBuilder UseCustomMvc(this IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "area_default",
                    template: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            return app;
        }
    }
}