using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mechanix.API.Core.Data.Models;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace Mechanix.API.Core.Data.Services
{
    public class PartService : IService<Part>
    {
        private readonly IMongoCollection<Part> _collection;
        private readonly IMemoryCache _cache;
        private const string CacheName = "PartData";

        public PartService(IDbConnection database, IMemoryCache cache)
        {
            _collection = database.PartCollection;
            _cache = cache;
        }

        public async Task CreateAsync(Part model)
        {
            await _collection.InsertOneAsync(model);
        }

        public async Task<List<Part>> GetAllAsync()
        {
            // Try to get all the parts from the cache.
            List<Part> parts = _cache.Get<List<Part>>(CacheName);

            // If the cache is empty, retrieve them from the database and load them into the cache.
            if(parts is null) {
                parts = await _collection.FindAsync(_ => true).Result.ToListAsync();
                _cache.Set(CacheName, parts, TimeSpan.FromDays(1));
            }

            return parts;
        }

        public async Task<List<Part>> GetAllFilteredAsync(Func<Part, bool> comparator)
        {
            // Try to get the filtered parts from the cache.
            List<Part> parts = _cache.Get<List<Part>>(CacheName).Where(part => comparator(part)).ToList();

            // If the cache is empty, retrieve them from the database.
            return parts ?? await _collection.FindAsync(part => comparator(part)).Result.ToListAsync();
        }

        public async Task<Part> GetAsync(string id)
        {
            // Try to get the part with the given id from the cache.
            List<Part> parts = _cache.Get<List<Part>>(CacheName).Where(part => part.Id == id).ToList();

            // If the cache doesn't contain the part, retrieve it from the database.
            return parts.FirstOrDefault() ?? await _collection.FindAsync(part => part.Id == id).Result.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Part model)
        {
            await _collection.ReplaceOneAsync(part => part.Id == model.Id, model);
        }

        public async Task RemoveAsync(string id)
        {
            await _collection.DeleteOneAsync(part => part.Id == id);
        }
    }
}
