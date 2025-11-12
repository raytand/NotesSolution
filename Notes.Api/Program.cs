using Confluent.Kafka;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Notes.Application;
using Notes.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration["ConnectionStrings:Default"];
builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseSqlServer(connectionString));

var kafkaBroker = builder.Configuration["Kafka:BootstrapServers"] ?? "kafka:9092";
builder.Services.AddSingleton<IProducer<Null, string>>(
    new ProducerBuilder<Null, string>(new ProducerConfig
    {
        BootstrapServers = kafkaBroker,
        Acks = Acks.All,
        EnableIdempotence = true,
        MessageSendMaxRetries = 10,
        ReconnectBackoffMs = 500
    }).Build()
);

builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
    var retries = 10;
    while (retries > 0)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch (SqlException)
        {
            retries--;
            Console.WriteLine("Waiting for SQL Server...");
            Thread.Sleep(3000);
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
