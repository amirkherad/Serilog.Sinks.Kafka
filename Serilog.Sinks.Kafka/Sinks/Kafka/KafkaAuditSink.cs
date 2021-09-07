using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Kafka.Contracts;
using System;

namespace Serilog.Sinks.Kafka
{
    /// <summary>
    /// Writes log events as new message to Kafka cluster using Audit logic, meaning that each row is synchronously committed
    /// and any errors that occur are propagated to the caller.
    /// </summary>
    public class KafkaAuditSink : ILogEventSink, IDisposable
    {
        private readonly IKafkaProducer _kafkaProducer;
        private readonly string _topic;

        public KafkaAuditSink(KafkaConfiguration kafkaConfiguration, string topic)
        {
            _kafkaProducer = DependenciesFactory.CreateKafkaProducer(kafkaConfiguration);
            _topic = topic;
        }

        /// <summary>Emit the provided log event to the sink.</summary>
        /// <param name="logEvent">The log event to produce.</param>
        public void Emit(LogEvent logEvent) => _kafkaProducer.Produce(_topic, logEvent).Wait();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
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