using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;


var builder = WebApplication.CreateBuilder(args);
{
	//Thêm các dịch vụ được yêu cầu bới MVC Framework
	builder.Services.AddControllersWithViews();

	//Đăng kí các dịch vụ với DI Container
	builder.Services.AddDbContext<BlogDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")));

	builder.Services.AddScoped<IBlogRepository, BlogRepository>();
	builder.Services.AddScoped<IDataSeeder, IDataSeeder>();
}
var app = builder.Build();
{
	//Cấu hình HTTPS Request pipeline
	//Thêm middleware để hiển thị thông tin báo lỗi
	if (app.Environment.IsDevelopment())
	{
		app.UseDeveloperExceptionPage();
	}
	else
	{
		app.UseExceptionHandler("/Blog/Error");
		//Thêm middleware cho việc áp dụng HSTS(Thêm header strict-transport-security vào HTTP response)
		app.UseHsts();
	}
	app.UseHttpsRedirection();
	app.UseStaticFiles();
	app.UseRouting();

	app.MapControllerRoute(name: "default",
		pattern: "{controller=Blog}/{action=Index}/{id?}");
}

//Thêm dữ liệu mẫu vào CSDL
using (var scope=app.Services.CreateScope())
{
	var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
	seeder.Initialize();
}	

app.Run();
