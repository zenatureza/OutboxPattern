using Microsoft.AspNetCore.Hosting;
using OutboxPattern.Jobs;
using OutboxPattern.Persistence;
using Quartz;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddQuartz(configure =>
{
    var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
    configure
        .AddJob<ProcessOutboxMessagesJob>(jobKey)
        .AddTrigger(trigger => 
            trigger
                .ForJob(jobKey)
                .WithSimpleSchedule(schedule => 
                    schedule
                        .WithIntervalInSeconds(10)
                        .RepeatForever()));

    configure.UseMicrosoftDependencyInjectionJobFactory();
});
builder.Services.AddQuartzHostedService();

builder.AddDbServices();

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

DatabaseManagementService.MigrationInitialisation(app);

app.Run();
