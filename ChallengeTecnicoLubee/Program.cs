using ChallengeTecnicoLubee.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Connection");

var pathDestino = Path.Combine(Directory.GetCurrentDirectory(), "images/publications");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Verificar y crear la carpeta "images" si no existe
string contentRootPath = app.Environment.ContentRootPath;

string imagesFolder = Path.Combine(contentRootPath, "images");
if (!Directory.Exists(imagesFolder))
{
    Directory.CreateDirectory(imagesFolder);
}

// Verificar y crear la carpeta "publications" si no existe
string publicationsFolder = Path.Combine(contentRootPath, "images", "publications");
if (!Directory.Exists(publicationsFolder))
{
    Directory.CreateDirectory(publicationsFolder);
}


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(publicationsFolder),
    RequestPath = "/images/publications"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("VueCorsPolicy");

app.Run();
