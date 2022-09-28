FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY data/API API
WORKDIR /app/API

COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/API/out .

RUN apt-get update
RUN apt-get install -y curl

# NEW CONFIGURATION: 

# Install Tracer
RUN curl -LO https://github.com/DataDog/dd-trace-dotnet/releases/download/v2.15.0/datadog-dotnet-apm_2.15.0_amd64.deb
RUN dpkg -i ./datadog-dotnet-apm_2.15.0_amd64.deb

# Set environment variables
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV DD_INTEGRATIONS=/opt/datadog/integrations.json
ENV DD_DOTNET_TRACER_HOME=/opt/datadog

# Variables needed so tracer knows what host to send the traces to(Port one is optional since this is the same value by default)
ENV DD_AGENT_HOST=datadog-agent
ENV DD_TRACE_AGENT_PORT=8126

# Unified Service Tagging env vars
ENV DD_ENV="test" 
ENV DD_SERVICE="Expert Finder" 
ENV DD_VERSION="1.0" 

#Script to create the directory for the log files
RUN /opt/datadog/createLogPath.sh

CMD dotnet build

ENTRYPOINT ["dotnet", "API.dll"]