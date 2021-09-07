using Serilog.Events;
using System.Threading.Tasks;

namespace Serilog.Sinks.Kafka.Contracts
{
    internal interface IKafkaProducer
    {
        Task Produce(string topic, LogEvent logEvent);
    }
}