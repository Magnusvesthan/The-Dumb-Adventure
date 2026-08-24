using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLiveServer", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5500",
                "http://localhost:5501",
                "http://127.0.0.1:5500",
                "http://127.0.0.1:5501",
                "http://localhost:5250",
                "http://127.0.0.1:5250")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = ""
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = ""
});

app.UseCors("AllowLiveServer");
app.MapGet("/", () => Results.Redirect("/index.html"));
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
