namespace Serilog.Sinks.Kafka
{
    public class KafkaConfiguration
    {
        public string[] BootstrapServers { get; set; }
        public string Topic { get; set; }
        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
        
        public KafkaConfiguration()
        {

        }

        public KafkaConfiguration(string[] bootstrapServers, string topic, string saslUsername = null, string saslPassword = null)
        {
            BootstrapServers = bootstrapServers;
            Topic = topic;
            SaslUsername = saslUsername;
            SaslPassword = saslPassword;
        }
    }
}