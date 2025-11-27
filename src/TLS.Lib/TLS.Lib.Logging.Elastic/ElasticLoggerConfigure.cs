using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Application;

namespace TLS.Lib.Logging.Elastic
{
    public static class ElasticLoggerConfigure
    {
        private const string DefaultElasticLoggerConfigKey = "ElasticLogger";

        /// <summary>
        /// Configure Serilog as the logging provider use default config section ["ElasticLogger"]
        /// and write it to Debug, Console and Elasticsearch
        /// </summary>
        /// <param name="builder">required</param>
        /// <returns></returns>
        public static IHostBuilder ConfigureLogger(this IHostBuilder builder)
        {
            return ConfigureLogger(builder, null, null);
        }

        /// <summary>
        /// Configure Serilog as the logging provider use special nodes url (separate by [;])
        /// and write it to Debug, Console and Elasticsearch
        /// </summary>
        /// <param name="builder">required</param>
        /// <param name="nodes">required</param>
        /// <returns></returns>
        public static IHostBuilder ConfigureLogger(this IHostBuilder builder, string nodes)
        {
            // Check param
            if (string.IsNullOrEmpty(nodes))
            {
                throw new ArgumentNullException(nameof(nodes));
            }
            return ConfigureLogger(builder, nodes, null);
        }

        /// <summary>
        /// Configure Serilog as the logging provider use default config section ["ElasticLogger"] with action setting for elasticsearch sink options
        /// and write it to Debug, Console and Elasticsearch
        /// </summary>
        /// <param name="builder">required</param>
        /// <param name="setupElasticOptions">required</param>
        /// <returns></returns>
        public static IHostBuilder ConfigureLogger(this IHostBuilder builder, Action<ElasticsearchSinkOptions> setupElasticOptions)
        {
            // Check param
            if (setupElasticOptions == null)
            {
                throw new ArgumentNullException(nameof(setupElasticOptions));
            }
            return ConfigureLogger(builder, null, setupElasticOptions);
        }

        /// <summary>
        /// Configure Serilog as the logging provider
        /// and write it to Debug, Console and Elasticsearch
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="nodes"></param>
        /// <param name="setupElasticOptions"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureLogger(this IHostBuilder builder, string nodes, Action<ElasticsearchSinkOptions> setupElasticOptions)
        {
            // Check param
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.UseSerilog((context, services, logger) => {
                //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var projectDLL = Assembly.GetEntryAssembly();
                var appSettings = services.GetRequiredService<IOptions<ApplicationSettings>>().Value;
                //var projectName = context.Configuration.GetSetting("ProjectName");
                //var projectVersion = context.Configuration.GetSetting("ProjectVersion");
                var projectName = string.IsNullOrEmpty(appSettings.ProjectName) ? projectDLL.GetName().Name : appSettings.ProjectName;
                var projectVersion = string.IsNullOrEmpty(appSettings.ProjectVersion) ? projectDLL.GetName().Version.ToString() : appSettings.ProjectVersion;

                // Load elastic search settings
                ElasticLoggerSettings elasticSettings;
                var section = context.Configuration.GetSection(DefaultElasticLoggerConfigKey);
                if (section.Exists())
                {
                    // Get from configuration
                    elasticSettings = section.Get<ElasticLoggerSettings>();
                    if (!string.IsNullOrEmpty(nodes))
                    {
                        elasticSettings.Nodes = nodes;
                    }

                    // Check if not config nodes
                    if (string.IsNullOrEmpty(elasticSettings.Nodes))
                    {
                        throw new Exception($"Elastic search settings of [nodes] was not found in your configuration section [{section.Key}]");
                    }
                }
                else
                {
                    // Check if not config nodes
                    if (string.IsNullOrEmpty(nodes))
                    {
                        throw new ArgumentNullException(nameof(nodes));
                    }
                    elasticSettings = new ElasticLoggerSettings { 
                        Nodes = nodes
                    };
                }

                // Get ElasticsearchSinkOptions
                var elasticsearchSinkOptions = BuildElasticSinkOptions(elasticSettings, setupElasticOptions, appSettings);

                // Configure logger
                logger
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("Application", appSettings.Application)
                .Enrich.WithProperty("ProjectName", projectName)
                .Enrich.WithProperty("ProjectVersion", projectVersion)
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(elasticsearchSinkOptions)
                ;//.ReadFrom.Configuration(context.Configuration);
            });
        }

        private static ElasticsearchSinkOptions BuildElasticSinkOptions(ElasticLoggerSettings elasticSettings, Action<ElasticsearchSinkOptions> setupElasticOptions, ApplicationSettings appSettings)
        {
            var nodes = elasticSettings.Nodes.Split(new char[] { ',', ';' }, StringSplitOptions.TrimEntries)
                .Select(url => new Uri(url));
            //var applicationName = configuration.GetSetting("Application");
            //var indexName = configuration.GetValue<string>("ElasticConfiguration:Index");
            //indexName = string.IsNullOrEmpty(indexName) ? appSettings.Application : indexName;

            var sinkOptions = new ElasticsearchSinkOptions(nodes);

            // InlineFields
            if (elasticSettings.InlineFields.HasValue)
            {
                sinkOptions.InlineFields = elasticSettings.InlineFields.Value;
            }

            // MinimumLogEventLevel
            if (elasticSettings.MinimumLogEventLevel.HasValue)
            {
                sinkOptions.MinimumLogEventLevel = elasticSettings.MinimumLogEventLevel.Value;
            }

            // BufferBaseFilename
            if (!string.IsNullOrEmpty(elasticSettings.BufferBaseFilename))
            {
                sinkOptions.BufferBaseFilename = elasticSettings.BufferBaseFilename;
            }

            // BufferFileSizeLimitBytes
            if (elasticSettings.BufferFileSizeLimitBytes.HasValue)
            {
                sinkOptions.BufferFileSizeLimitBytes = elasticSettings.BufferFileSizeLimitBytes.Value;
            }

            // BufferRetainedInvalidPayloadsLimitBytes
            if (elasticSettings.BufferRetainedInvalidPayloadsLimitBytes.HasValue)
            {
                sinkOptions.BufferRetainedInvalidPayloadsLimitBytes = elasticSettings.BufferRetainedInvalidPayloadsLimitBytes.Value;
            }

            // EmitEventFailure
            if (elasticSettings.EmitEventFailure.HasValue)
            {
                sinkOptions.EmitEventFailure = elasticSettings.EmitEventFailure.Value;
            }

            // QueueSizeLimit
            if (elasticSettings.QueueSizeLimit.HasValue)
            {
                sinkOptions.QueueSizeLimit = elasticSettings.QueueSizeLimit.Value;
            }

            // BufferFileCountLimit
            if (elasticSettings.BufferFileCountLimit.HasValue)
            {
                sinkOptions.BufferFileCountLimit = elasticSettings.BufferFileCountLimit.Value;
            }

            // FormatStackTraceAsArray
            if (elasticSettings.FormatStackTraceAsArray.HasValue)
            {
                sinkOptions.FormatStackTraceAsArray = elasticSettings.FormatStackTraceAsArray.Value;
            }

            // BufferLogShippingInterval
            if (elasticSettings.BufferLogShippingInterval.HasValue)
            {
                sinkOptions.BufferLogShippingInterval = elasticSettings.BufferLogShippingInterval.Value;
            }

            // ConnectionTimeout
            if (elasticSettings.ConnectionTimeout.HasValue)
            {
                sinkOptions.ConnectionTimeout = elasticSettings.ConnectionTimeout.Value;
            }

            // Period
            if (elasticSettings.Period.HasValue)
            {
                sinkOptions.Period = elasticSettings.Period.Value;
            }

            // AutoRegisterTemplate
            if (elasticSettings.AutoRegisterTemplate.HasValue)
            {
                sinkOptions.AutoRegisterTemplate = elasticSettings.AutoRegisterTemplate.Value;
            }
            else
            {
                sinkOptions.AutoRegisterTemplate = true;
            }

            // AutoRegisterTemplateVersion
            if (elasticSettings.AutoRegisterTemplateVersion.HasValue)
            {
                sinkOptions.AutoRegisterTemplateVersion = elasticSettings.AutoRegisterTemplateVersion.Value;
            }

            // BufferRetainedInvalidPayloadsLimitBytes
            if (elasticSettings.RegisterTemplateFailure.HasValue)
            {
                sinkOptions.RegisterTemplateFailure = elasticSettings.RegisterTemplateFailure.Value;
            }

            // TemplateName
            if (!string.IsNullOrEmpty(elasticSettings.TemplateName))
            {
                sinkOptions.TemplateName = elasticSettings.TemplateName;
            }

            // TemplateCustomSettings
            if (elasticSettings.TemplateCustomSettings != null && elasticSettings.TemplateCustomSettings.Count > 0)
            {
                sinkOptions.TemplateCustomSettings = elasticSettings.TemplateCustomSettings;
            }

            // OverwriteTemplate
            if (elasticSettings.OverwriteTemplate.HasValue)
            {
                sinkOptions.OverwriteTemplate = elasticSettings.OverwriteTemplate.Value;
            }

            // NumberOfShards
            if (elasticSettings.NumberOfShards.HasValue)
            {
                sinkOptions.NumberOfShards = elasticSettings.NumberOfShards.Value;
            }

            // NumberOfReplicas
            if (elasticSettings.NumberOfReplicas.HasValue)
            {
                sinkOptions.NumberOfReplicas = elasticSettings.NumberOfReplicas.Value;
            }

            // IndexAliases
            if (elasticSettings.IndexAliases != null && elasticSettings.IndexAliases.Length > 0)
            {
                sinkOptions.IndexAliases = elasticSettings.IndexAliases;
            }

            // IndexFormat
            if (!string.IsNullOrEmpty(elasticSettings.IndexFormat))
            {
                sinkOptions.IndexFormat = elasticSettings.IndexFormat;
            }
            else
            {
                sinkOptions.IndexFormat = $"{appSettings.Application}-{{0:yyyy.MM.dd}}";
            }

            // DeadLetterIndexName
            if (!string.IsNullOrEmpty(elasticSettings.DeadLetterIndexName))
            {
                sinkOptions.DeadLetterIndexName = elasticSettings.DeadLetterIndexName;
            }

            // TypeName
            if (!string.IsNullOrEmpty(elasticSettings.TypeName))
            {
                sinkOptions.TypeName = elasticSettings.TypeName;
            }

            // BatchAction
            if (elasticSettings.BatchAction.HasValue)
            {
                sinkOptions.BatchAction = elasticSettings.BatchAction.Value;
            }

            // PipelineName
            if (!string.IsNullOrEmpty(elasticSettings.PipelineName))
            {
                sinkOptions.PipelineName = elasticSettings.PipelineName;
            }

            // BatchPostingLimit
            if (elasticSettings.BatchPostingLimit.HasValue)
            {
                sinkOptions.BatchPostingLimit = elasticSettings.BatchPostingLimit.Value;
            }
            // SingleEventSizePostingLimit
            if (elasticSettings.SingleEventSizePostingLimit.HasValue)
            {
                sinkOptions.SingleEventSizePostingLimit = elasticSettings.SingleEventSizePostingLimit.Value;
            }
            // DetectElasticsearchVersion
            if (elasticSettings.DetectElasticsearchVersion.HasValue)
            {
                sinkOptions.DetectElasticsearchVersion = elasticSettings.DetectElasticsearchVersion.Value;
            }

            // Invoke action of elastic sink options
            if (setupElasticOptions != null)
            {
                setupElasticOptions(sinkOptions);
            }

            return sinkOptions;
        }
    }
}
