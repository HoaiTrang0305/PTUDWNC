using Mapster;
using TatBlog.Core.Entities;
using TatBlog.Core.DTO;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.Services.Blogs;
using MapsterMapper;

namespace TatBlog.WebApp.Mapsters
{
    public static class MapsterDependencyInjection
    {
        public static WebApplicationBuilder ConfigureMapster(
            this WebApplicationBuilder builder)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(MapsterConfiguration).Assembly);

            builder.Services.AddSingleton(config);
            builder.Services.AddScoped<IMapper, ServiceMapper>();

            return builder;
        }
    }
}
