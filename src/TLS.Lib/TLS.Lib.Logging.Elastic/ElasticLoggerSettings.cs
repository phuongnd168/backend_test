using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Logging.Elastic
{
    public class ElasticLoggerSettings
    {
        public string Nodes { get; set; }
        public bool? InlineFields { get; set; }
        public LogEventLevel? MinimumLogEventLevel { get; set; }
        public string BufferBaseFilename { get; set; }
        public long? BufferFileSizeLimitBytes { get; set; }
        public long? BufferRetainedInvalidPayloadsLimitBytes { get; set; }
        public EmitEventFailureHandling? EmitEventFailure { get; set; }
        public int? QueueSizeLimit { get; set; }
        public int? BufferFileCountLimit { get; set; }
        public bool? FormatStackTraceAsArray { get; set; }
        public TimeSpan? BufferLogShippingInterval { get; set; }
        public TimeSpan? ConnectionTimeout { get; set; }
        public TimeSpan? Period { get; set; }
        public bool? AutoRegisterTemplate { get; set; } = true;
        public AutoRegisterTemplateVersion? AutoRegisterTemplateVersion { get; set; }
        public RegisterTemplateRecovery? RegisterTemplateFailure { get; set; }
        public string TemplateName { get; set; }
        public Dictionary<string, string> TemplateCustomSettings { get; set; }
        public bool? OverwriteTemplate { get; set; }
        public int? NumberOfShards { get; set; }
        public int? NumberOfReplicas { get; set; }
        public string[] IndexAliases { get; set; }
        public string IndexFormat { get; set; }
        public string DeadLetterIndexName { get; set; }
        public string TypeName { get; set; }
        public ElasticOpType? BatchAction { get; set; }
        public string PipelineName { get; set; }
        public int? BatchPostingLimit { get; set; }
        public long? SingleEventSizePostingLimit { get; set; }
        public bool? DetectElasticsearchVersion { get; set; }
    }
}
