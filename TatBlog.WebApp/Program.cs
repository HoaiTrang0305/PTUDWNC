var builder = WebApplication.CreateBuilder(args);
{
	//Thêm các dịch vụ được yêu cầu bới MVC Framework
	builder.Services.AddControllersWithViews();
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

app.Run();
