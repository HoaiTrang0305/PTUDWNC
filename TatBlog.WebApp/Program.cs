using TatBlog.WebApp.Extensions;
using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Mapsters;

var buider = WebApplication.CreateBuilder(args);
{
	buider
		.ConfigureMvc()
		.ConfigureServices()
		.ConfigureMapster();
}

var app = buider.Build();
{
	app.UseRequestPipeLine();
	app.UseBlogRoutes();
	app.UseDataSeeder();
}
app.Run();