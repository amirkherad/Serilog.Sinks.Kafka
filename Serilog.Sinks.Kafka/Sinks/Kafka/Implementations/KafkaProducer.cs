using Confluent.Kafka;
using Newtonsoft.Json;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.Kafka.Contracts;
using System.Net;
using System.Threading.Tasks;

namespace Serilog.Sinks.Kafka.Implementations
{
    internal class KafkaProducer : IKafkaProducer
    {
        private readonly ProducerConfig _producerConfig;

        public KafkaProducer(KafkaConfiguration kafkaConfiguration)
        {
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = string.Join(',', kafkaConfiguration.BootstrapServers),
                ClientId = Dns.GetHostName(),
                SaslUsername = kafkaConfiguration.SaslUsername,
                SaslPassword = kafkaConfiguration.SaslPassword,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.Plaintext
            };
        }

        public async Task Produce(string topic, LogEvent logEvent)
        {
            using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
            var message = JsonConvert.SerializeObject(logEvent);
            var result = producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
            await result.ContinueWith(task =>
            {
                if (task.IsFaulted)
                    SelfLog.WriteLine("Topic: {0}, Message: {1}, Error: {2}", topic, message, result.Exception.Message);
            });
        }
    }
}