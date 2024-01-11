//Ejemplo de conexi�n, esto debiese ir en los controllers, pero lo dej� aqu� ya que solo se ped�a una conexi�n sencilla la primera semana.
string apiResponse = "";
try
{
    using (var httpClient = new HttpClient())
    {
        using (var response = await httpClient.GetAsync("http://localhost/api/WorkOrder"))
        {
            apiResponse = await response.Content.ReadAsStringAsync();
        }
    }
} catch (Exception ex)
{
   apiResponse = ex.Message;
}


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Output: " + apiResponse);

app.Run();
