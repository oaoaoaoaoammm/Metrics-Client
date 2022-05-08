using MetricsAgent;
using WorkWithBD;
using AutoMapper;
using FluentMigrator.Runner;
using Quartz;
using Quartz.Spi;
using Quartz.Impl;

clean();

var builder = WebApplication.CreateBuilder(args);
var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
var _mapper = mapperConfiguration.CreateMapper();

try
{
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

    builder.Services.AddControllers();

    builder.Services.AddHostedService<QuartzHostedService>();

    builder.Services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString("Data Source=metrics.db")
                    .ScanIn(typeof(Program).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

    builder.Services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
    builder.Services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
    builder.Services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();
    builder.Services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
    builder.Services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();

    builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
    builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

    builder.Services.AddSingleton<CpuMetricJob>();
    builder.Services.AddSingleton(new JobSchedule(jobType: typeof(CpuMetricJob), cronExpression: "0/1 * * * * ?"));

    builder.Services.AddSingleton<RamMetricJob>();
    builder.Services.AddSingleton(new JobSchedule(jobType: typeof(RamMetricJob), cronExpression: "0/1 * * * * ?"));

    builder.Services.AddSingleton<NetworkMetricJob>();
    builder.Services.AddSingleton(new JobSchedule(jobType: typeof(NetworkMetricJob), cronExpression: "0/1 * * * * ?"));

    builder.Services.AddSingleton<HddMetricJob>();
    builder.Services.AddSingleton(new JobSchedule(jobType: typeof(HddMetricJob), cronExpression: "0/1 * * * * ?"));

    builder.Services.AddSingleton<DotNetMetricJob>();
    builder.Services.AddSingleton(new JobSchedule(jobType: typeof(DotNetMetricJob), cronExpression: "0/1 * * * * ?"));

    builder.Services.AddSingleton(_mapper);

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        scope.ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
}
finally
{
}


void clean()
{
    string directory = Directory.GetCurrentDirectory();
    File.Delete(directory + "/metrics.db");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Data base Updated!");
    Console.ResetColor();
}