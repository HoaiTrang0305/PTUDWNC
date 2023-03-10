using TatBlog.WebApp.Extensions;

var buider = WebApplication.CreateBuilder(args);
{
	buider
		.ConfigureMvc()
		.ConfigureServices();
}

var app = buider.Build();
{
	app.UseRequestPipeLine();
	app.UseBlogRoutes();
	app.UseDataSeeder();
}
app.Run();