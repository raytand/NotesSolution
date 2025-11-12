namespace Notes.Kafka.Handlers
{
    public interface IKafkaHandler<T>
    {
        Task Handle(T message, CancellationToken ct);
    }
}
