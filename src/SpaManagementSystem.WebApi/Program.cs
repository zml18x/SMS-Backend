var builder = WebApplication.CreateBuilder(args);

SpaManagementSystem.Infrastructure.Container.InfrastructureDependencies.ConfigureServices(builder.Services,
    builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

SpaManagementSystem.Infrastructure.Data.DatabaseMigrator.MigrateDatabase(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
