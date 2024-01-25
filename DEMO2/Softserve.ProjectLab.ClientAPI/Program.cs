using Softserve.ProjectLab.ClientAPI.Services;
using Softserve.ProjectLab.ClientAPI.Config;

var builder = WebApplication.CreateBuilder(args);

// commenting this line because it is redundant with .AddControllersWithViews()
// builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure a HttpClient with a common BaseAddress for all services
builder.Services.AddHttpClient("apiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost"); // URL for  API
});

// This is where you would register your custom services
builder.Services.AddScoped<ApiConnector>();
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

//exclusively for views
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
