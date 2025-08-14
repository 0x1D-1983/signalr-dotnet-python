using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SignalRFilterPlay.Data;
using SignalRFilterPlay.Services;
using SignalRFilterPlay.Hubs;

var builder = WebApplication.CreateBuilder(args);

// SignalR with camelCase JSON so Python can send {"term": "..."}
builder.Services
    .AddSignalR()
    .AddJsonProtocol(o =>
    {
        o.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Very open CORS for dev only
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true)));

builder.Services.AddSingleton<ObservableStream<EpexTrade>>();
builder.Services.AddHostedService<TradesHubStreamer>();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
});

var app = builder.Build();
app.UseCors();

app.MapGet("/", () => "OK");
app.MapHub<TradesHub>("/trades", options =>
{
    
    // options.EnableDetailedErrors = true;
});

app.Run();