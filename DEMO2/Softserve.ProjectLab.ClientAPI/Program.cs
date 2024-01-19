using Softserve.ProjectLab.ClientAPI.Services;
using Softserve.ProjectLab.ClientAPI.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure a HttpClient with a common BaseAddress for all services
builder.Services.AddHttpClient("apiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost"); // URL for  API
});

//builder.Configuration.AddJsonFile("appsettings.json", optional: false);

//// Configure API keys or tokens
//builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// This is where you would register your custom services
builder.Services.AddScoped<ApiConnector>();
builder.Services.AddScoped<IWorkOrderService, WorkOrderService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<ITechnicianService, TechnicianService>();
builder.Services.AddScoped<IWorkTypeService, WorkTypeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
