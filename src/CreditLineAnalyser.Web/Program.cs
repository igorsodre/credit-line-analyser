using CreditLineAnalyser.Web.Interfaces;
using CreditLineAnalyser.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Scan(
    scan => {
        scan.FromAssemblyOf<Program>()
            .AddClasses(classes => classes.AssignableTo<ICreditLineAnalyser>())
            .AsSelf()
            .WithScopedLifetime();
    }
);
builder.Services.AddSingleton<ICreditLineProcessor, CreditLineProcessor>();

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
