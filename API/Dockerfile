FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5420

ENV ASPNETCORE_URLS=http://+:5420

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["API/API.csproj", "API/"]
RUN dotnet restore "API/API.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# NEW CONFIGURATION: 

# Please select the corresponding download for your Linux containers https://github.com/DataDog/dd-trace-dotnet/releases/latest

# Download and install the Tracer
RUN mkdir -p /opt/datadog \
    && mkdir -p /var/log/datadog \
    && TRACER_VERSION=$(curl -s https://api.github.com/repos/DataDog/dd-trace-dotnet/releases/latest | grep tag_name | cut -d '"' -f 4 | cut -c2-) \
    && curl -LO https://github.com/DataDog/dd-trace-dotnet/releases/download/v${TRACER_VERSION}/datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb \
    && dpkg -i ./datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb \
    && rm ./datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb

# Enable the tracer
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV DD_DOTNET_TRACER_HOME=/opt/datadog
ENV DD_INTEGRATIONS=/opt/datadog/integrations.json

# Variables needed so tracer knows where to send the traces to
ENV DD_AGENT_HOST=datadog-agent
ENV DD_TRACE_AGENT_PORT=8126

#Script to create the directory for the log files
RUN /opt/datadog/createLogPath.sh

ENTRYPOINT ["dotnet", "API.dll"]