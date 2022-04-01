using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mechanix.Core.Data.Services
{
    interface IService<T>
    {
        public Task<T> GetAsync(string id);
        public Task<List<T>> GetAllAsync();
        public Task<List<T>> GetAllFilteredAsync(Func<T, bool> comparator);
        public Task Create(T model);
        public Task Update(T model);
    }
}
