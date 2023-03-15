using TatBlog.WebApp.Extensions;
using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Mapsters;
using TatBlog.WebApp.Validations;

var buider = WebApplication.CreateBuilder(args);
{
	buider
		.ConfigureMvc()
		.ConfigureServices()
		.ConfigureMapster()
		.ConfigureFluentValidation();
}

var app = buider.Build();
{
	app.UseRequestPipeLine();
	app.UseBlogRoutes();
	app.UseDataSeeder();
}
app.Run();