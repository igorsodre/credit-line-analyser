using CreditLineAnalyser.Web.Filters;
using CreditLineAnalyser.Web.Interfaces;
using CreditLineAnalyser.Web.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.Filters.Add<ThrottlingFilter>());
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
builder.Services.AddSingleton<IConnectionMultiplexer>(
    x => ConnectionMultiplexer.Connect(
        builder.Configuration.GetValue<string>("RedisSettings:ConnectionString"),
        options => {
            options.ConnectRetry = 10;
            options.AbortOnConnectFail = false;
        }
    )
);
builder.Services.AddSingleton<IThrottleService, ThrottleService>();
builder.Services.AddSingleton<ICreditLineProcessor, CreditLineProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
