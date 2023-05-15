using ApiBibliotecas.Data;
using ApiBibliotecas.Helpers;
using ApiBibliotecas.Repositorys;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient(builder.Configuration.GetSection("KeyVault"));
});
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret keyVaultSecret = await secretClient.GetSecretAsync("SqlAzure");
string connectionString = keyVaultSecret.Value;

builder.Services.AddTransient<BibliotecasRepository>();
builder.Services.AddDbContext<BibliotecasContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddSingleton<HelperOAuthToken>();
HelperOAuthToken helper = new HelperOAuthToken(builder.Configuration);
builder.Services.AddAuthentication(helper.GetAuthenticationOptions()).AddJwtBearer(helper.GetJwtOptions());

// Add services to the container.

builder.Services.AddControllers();
//Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api CRUD Bibliotecas",
        Description = "Proyecto Bibliotecas",
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "CrudBibliotecas");
    options.RoutePrefix = "";
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
