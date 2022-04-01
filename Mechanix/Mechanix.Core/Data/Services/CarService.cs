using Mechanix.Core.Data.Models;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mechanix.Core.Data.Services
{
    public class CarService : IService<Car>
    {
        private readonly IMongoCollection<Car> _collection;
        private readonly IMemoryCache _cache;
        private const string CacheName = "CarData";

        public CarService(IDbConnection database, IMemoryCache cache)
        {
            _collection = database.CarCollection;
            _cache = cache;
        }

        public async Task Create(Car model)
        {
            await _collection.InsertOneAsync(model);
        }

        public async Task<List<Car>> GetAllAsync()
        {
            // Try to get all the cars in the cache.
            List<Car> cars = _cache.Get<List<Car>>(CacheName);

            // If the cache is empty, retrieve them from the database and load them into the cache.
            if(cars is null) {
                cars = await _collection.FindAsync(_ => true).Result.ToListAsync();
                _cache.Set(CacheName, cars, TimeSpan.FromDays(1));
            }

            return cars;
        }

        public async Task<List<Car>> GetAllFilteredAsync(Func<Car, bool> comparator)
        {
            // Try to get all the filtered cars in the cache.
            List<Car> cars = _cache.Get<List<Car>>(CacheName).Where(car => comparator(car)).ToList();
            
            // If the cache is empty, retrieve them from the database.
            return cars ?? await _collection.FindAsync(car => comparator(car)).Result.ToListAsync();
        }

        public async Task<Car> GetAsync(string id)
        {
            // Search the cache for a car with the given id.
            IEnumerable<Car> cars = _cache.Get<List<Car>>(CacheName).Where(car => car.Id == id);

            // If the cache doesn't contain a car with the given id, retrieve it from the database.
            return cars.FirstOrDefault() ?? await _collection.FindAsync(car => car.Id == id).Result.FirstOrDefaultAsync();
        }

        public async Task Update(Car model)
        {
            await _collection.ReplaceOneAsync(car => car.Id == model.Id, model);
        }
    }
}
