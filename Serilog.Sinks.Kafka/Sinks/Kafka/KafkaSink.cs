using Serilog.Events;
using Serilog.Sinks.Kafka.Contracts;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serilog.Sinks.Kafka
{
    class KafkaSink : IBatchedLogEventSink, IDisposable
    {
        private readonly string _topic;
        private readonly IKafkaProducer _kafkaProducer;

        public KafkaSink(string topic, KafkaConfiguration kafkaConfiguration)
        {
            _topic = topic;
            _kafkaProducer = DependenciesFactory.CreateKafkaProducer(kafkaConfiguration);
        }

        public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
        {
            foreach (var logEvent in batch)
                await _kafkaProducer.Produce(_topic, logEvent);
        }

        public async Task OnEmptyBatchAsync()
        {
            await EmitBatchAsync(Enumerable.Empty<LogEvent>());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the Serilog.Sinks.Kafka.KafkaAuditSink and optionally
        /// releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            // This class needn't to dispose anything. This is just here for sink interface compatibility.
        }
    }
}
