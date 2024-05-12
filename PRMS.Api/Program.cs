using Microsoft.EntityFrameworkCore;
using PRMS.Api.Extensions;
using PRMS.Api.Middlewares;
using PRMS.Data.Contexts;
using PRMS.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbServices(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddDbContext<AppDbContext>(option => option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAllOrigins");
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await Seeder.Run(app);

app.Run();