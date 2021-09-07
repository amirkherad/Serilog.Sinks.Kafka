using Serilog.Sinks.Kafka.Contracts;
using Serilog.Sinks.Kafka.Implementations;

namespace Serilog.Sinks.Kafka
{
    internal static class DependenciesFactory
    {
        public static IKafkaProducer CreateKafkaProducer(KafkaConfiguration kafkaConfiguration)
        {
            return new KafkaProducer(kafkaConfiguration);
        }
    }
}