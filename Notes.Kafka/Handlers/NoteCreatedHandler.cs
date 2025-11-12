using Notes.Kafka.Contracts;


namespace Notes.Kafka.Handlers
{

    public class NoteCreatedHandler : IKafkaHandler<string>
    {
        private readonly string _logPath;

        public NoteCreatedHandler()
        {
            _logPath = Path.Combine(AppContext.BaseDirectory, "Logs", "kafka.log");
            Directory.CreateDirectory(Path.GetDirectoryName(_logPath)!);
        }

        public async Task Handle(string message, CancellationToken ct)
        {
            Console.WriteLine(message);
            await File.AppendAllLinesAsync("Logs/kafka.log", new[] { message }, ct);
        }
    }
}
