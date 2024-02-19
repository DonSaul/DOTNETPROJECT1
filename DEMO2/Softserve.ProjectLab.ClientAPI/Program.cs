using Softserve.ProjectLab.ClientAPI.Services;
using Softserve.ProjectLab.ClientAPI.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var configuration = builder.Configuration;

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHsts(options =>
    {
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromDays(365);
        options.Preload = true;
    });
}

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("https://localhost", "http://localhost:5173")
			   .AllowAnyHeader()
               .AllowAnyMethod()
               .WithExposedHeaders("Access-Control-Allow-Origin");

	});
});

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure a HttpClient with a common BaseAddress for all services
builder.Services.AddHttpClient("apiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost"); // URL for  API
});

// This is where you would register your custom services
builder.Services.AddScoped<IApiConnector, ApiConnector>();
builder.Services.AddScoped<IWorkOrderService, WorkOrderService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<ITechnicianService, TechnicianService>();
builder.Services.AddScoped<IWorkTypeService, WorkTypeService>();
builder.Services.AddScoped<IWorkOrderDetailsService, WorkOrderDetailsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // view error page
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

//exclusively for views
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.UseHttpsRedirection();
app.UseAuthentication();
app.MapControllers();

app.Run();
