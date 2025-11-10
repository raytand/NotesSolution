using Confluent.Kafka;

var broker = Environment.GetEnvironmentVariable("Kafka__BootstrapServers") ?? "kafka:9092";

var config = new ConsumerConfig
{
    BootstrapServers = broker,
    GroupId = "notes-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<Null, string>(config).Build();
consumer.Subscribe("notes-topic");

Console.WriteLine("Kafka consumer started...");

var logPath = Path.Combine(AppContext.BaseDirectory, "Logs", "kafka.log");
Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);

while (true)
{
    try
    {
        var cr = consumer.Consume();
        var msg = $"[{DateTime.UtcNow:O}] {cr.Message.Value}";
        Console.WriteLine(msg);
        File.AppendAllLines(logPath, new[] { msg });
    }
    catch (ConsumeException e)
    {
        Console.WriteLine($"Consume error: {e.Error.Reason}");
        Thread.Sleep(1000);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Thread.Sleep(1000);
    }
}
