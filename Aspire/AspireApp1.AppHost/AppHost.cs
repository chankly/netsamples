var builder = DistributedApplication.CreateBuilder(args);

var jaeger = builder.AddContainer("jaeger", "jaegertracing/all-in-one")
    .WithHttpEndpoint(port: 16686, targetPort: 16686, name: "ui")        // UI web
    .WithEndpoint(port: 4317, targetPort: 4317, name: "otlp-grpc")      // OTLP gRPC
    .WithEndpoint(port: 4318, targetPort: 4318, name: "otlp-http")      // OTLP HTTP
    .WithEndpoint(port: 6831, targetPort: 6831, name: "thrift-compact") // Thrift compact
    .WithEndpoint(port: 14268, targetPort: 14268, name: "thrift-http");

var apiService = builder.AddProject<Projects.AspireApp1_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WaitFor(jaeger);

builder.AddProject<Projects.AspireApp1_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();