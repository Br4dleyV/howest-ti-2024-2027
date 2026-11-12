using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;

namespace ReadingList.ServiceDefaults;

// Shares telemetry configuration between the APIs without sharing their business logic or HTTP models.
public static class Extensions
{
    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        // Include readable messages and scopes so structured logs can be linked to the current trace.
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry().WithTracing(tracing =>
        {
            // Keep all request traces during development so the complete workflow can be inspected.
            if (builder.Environment.IsDevelopment())
            {
                tracing.SetSampler(new AlwaysOnSampler());
            }

            tracing.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation(); // Follow incoming requests and outgoing API calls across processes
        });

        // Aspire supplies the dashboard endpoint and authentication through environment configuration.
        if (!string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter(); // Export logs and traces without a fixed dashboard address
        }

        return builder;
    }
}
