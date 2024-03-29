﻿// Serilog.Sinks.Kafka Copyright 2021 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Kafka;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.Kafka() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class KafkaLoggerConfigurationExtensions
    {
        public static LoggerConfiguration Kafka(this LoggerSinkConfiguration loggerSinkConfiguration,
            string[] bootstrapServers,
            string topic,
            string saslUsername = null,
            string saslPassword = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            if (loggerSinkConfiguration == null) throw new ArgumentNullException(nameof(loggerSinkConfiguration));
            if (string.IsNullOrWhiteSpace(topic)) throw new ArgumentNullException(nameof(topic));
            if (bootstrapServers == null) throw new ArgumentNullException(nameof(bootstrapServers));
            var kafkaConfiguration = new KafkaConfiguration(bootstrapServers, topic, saslUsername, saslPassword);
            var logEventSink = new KafkaAuditSink(kafkaConfiguration, topic);
            return loggerSinkConfiguration.Sink(logEventSink, restrictedToMinimumLevel);
        }
    }
}