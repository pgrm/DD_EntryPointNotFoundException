FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

ENTRYPOINT ["dotnet", "Not.Working.Api.dll"]
ENV ASPNETCORE_URLS=http://+:50007
ARG source=.

ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV DD_INTEGRATIONS=/opt/datadog/integrations.json
ENV DD_AGENT_HOST=datadog.local
ENV DD_TRACE_AGENT_PORT=8126

 # This needs to be overwritten and enabled in the ecs task definition for production
ENV DD_TRACE_ENABLED=true

RUN mkdir -p /opt/datadog
ARG TRACER_VERSION=1.1.0

RUN curl -L https://github.com/DataDog/dd-trace-dotnet/releases/download/v$TRACER_VERSION/datadog-dotnet-apm-$TRACER_VERSION.tar.gz | tar xzf - -C /opt/datadog

WORKDIR /app
EXPOSE 50007
COPY $source .