using Ambev.DeveloperEvaluation;
using Ambev.DeveloperEvaluation.Domain.Events;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.MongoDB.Repositories
{
    public class SaleEventRepository : BaseRepository<object>
    {
        public SaleEventRepository(MongoDbContext context)
            : base(context, "SaleEvents")
        {
        }

        public async Task StoreSaleCreatedEventAsync(SaleCreatedEvent @event)
        {
            await _collection.InsertOneAsync(@event);
        }

        public async Task StoreSaleModifiedEventAsync(SaleModifiedEvent @event)
        {
            await _collection.InsertOneAsync(@event);
        }

        public async Task StoreSaleCancelledEventAsync(SaleCancelledEvent @event)
        {
            await _collection.InsertOneAsync(@event);
        }

        public async Task StoreItemCancelledEventAsync(ItemCancelledEvent @event)
        {
            await _collection.InsertOneAsync(@event);
        }
    }
}