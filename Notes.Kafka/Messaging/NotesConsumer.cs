using Confluent.Kafka;
using Notes.Kafka.Handlers;

namespace Notes.Kafka.Messaging;

public class NotesConsumer
{
    private readonly IKafkaHandler<string> _handler;

    public NotesConsumer(IKafkaHandler<string> handler)
    {
        _handler = handler;
    }

    public async Task StartAsync(CancellationToken token)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = Environment.GetEnvironmentVariable("Kafka__BootstrapServers") ?? "kafka:9092",
            GroupId = "notes-group-1",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<Null, string>(config).Build();
        consumer.Subscribe("notes-topic-1");

        Console.WriteLine("Kafka consumer started...");
        try
        {
            while (true)
            {
                try
                {
                    var cr = consumer.Consume(token);
                    var message = cr.Message.Value;

                    
                    try
                    {
                        await _handler.Handle(message, token);
                        consumer.Commit(cr);
                    }
                    catch (Exception exHandler)
                    {
                        Console.WriteLine($"Handler error: {exHandler.Message}");
                    }
                }
                catch (ConsumeException ce)
                {
                    Console.WriteLine($"Consume error: {ce.Error.Reason}");
                    await Task.Delay(5000, token);
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Consumer cancelled.");
        }
        finally
        {
            consumer.Close();
        }
    }
}
