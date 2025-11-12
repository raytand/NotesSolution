using Notes.Kafka.Handlers;
using Notes.Kafka.Messaging;

namespace KafkaConsumerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new NoteCreatedHandler();
            var consumer = new NotesConsumer(handler);
            var cts = new CancellationTokenSource();

            Task.Run(() => consumer.StartAsync(cts.Token));

            Console.WriteLine("Kafka consumer running... Press Ctrl+C to exit.");
            
            using var waitHandle = new ManualResetEvent(false);
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                waitHandle.Set();
            };

            waitHandle.WaitOne();
        }
    }
}
