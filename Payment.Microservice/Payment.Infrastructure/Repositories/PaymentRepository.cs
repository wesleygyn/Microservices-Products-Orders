using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Payment.Domain.Interfaces.Repository;
using Payment.Infrastructure.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IMongoCollection<Payment.Domain.Entities.Payment> _collection;

    public PaymentRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _collection = database.GetCollection<Payment.Domain.Entities.Payment>(settings.Value.CollectionName);

        // Índice único por OrderId
        var indexKeys = Builders<Payment.Domain.Entities.Payment>.IndexKeys.Ascending(p => p.OrderId);
        var indexOptions = new CreateIndexOptions { Unique = false };
        _collection.Indexes.CreateOne(new CreateIndexModel<Payment.Domain.Entities.Payment>(indexKeys, indexOptions));
    }

    public async Task<Payment.Domain.Entities.Payment?> GetByIdAsync(string id)
        => await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task<Payment.Domain.Entities.Payment?> GetByOrderIdAsync(string orderId)
        => await _collection.Find(p => p.OrderId == orderId).FirstOrDefaultAsync();

    public async Task<IEnumerable<Payment.Domain.Entities.Payment>> GetAllAsync()
        => await _collection.Find(_ => true).SortByDescending(p => p.CreatedAt).ToListAsync();

    public async Task<Payment.Domain.Entities.Payment> AddAsync(Payment.Domain.Entities.Payment payment)
    {
        await _collection.InsertOneAsync(payment);
        return payment;
    }

    public async Task<Payment.Domain.Entities.Payment> UpdateAsync(Payment.Domain.Entities.Payment payment)
    {
        await _collection.ReplaceOneAsync(p => p.Id == payment.Id, payment);
        return payment;
    }
}