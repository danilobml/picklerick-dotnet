using Picklerick.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors((options) => {
    options.AddPolicy("DevCors", (corsBuilder) => {
        corsBuilder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
    options.AddPolicy("ProdCors", (corsBuilder) => {
        corsBuilder.WithOrigins("https://www.my-site.com")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
