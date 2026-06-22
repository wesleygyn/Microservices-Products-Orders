using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Payment.Application.Services.Interface;
using Payment.Application.Services.Service;
using Payment.Application.Settings;
using Payment.Domain.Interfaces.Repository;
using Payment.Infrastructure.HttpClients;
using Payment.Infrastructure.Repositories;
using Payment.Infrastructure.Settings;
using System;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new EnumSerializer<Payment.Domain.Enums.PaymentStatusEnum>(BsonType.String));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Registra AutoApproveSettings
builder.Services.Configure<AutoApproveSettings>(
    builder.Configuration.GetSection("AutoApprove"));

builder.Services.AddHttpClient<IOrdersHttpClient, OrdersHttpClient>(client =>
    client.BaseAddress = new Uri(builder.Configuration["OrdersApi:BaseUrl"]!));

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Payment API",
        Version = "v1",
        Description = "API de gerenciamento de pagamentos - Microserviço"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }