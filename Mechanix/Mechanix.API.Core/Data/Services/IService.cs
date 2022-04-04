using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mechanix.API.Core.Data.Services
{
    public interface IService<T>
    {
        public Task<T> GetAsync(string id);
        public Task<List<T>> GetAllAsync();
        public Task<List<T>> GetAllFilteredAsync(Func<T, bool> comparator);
        public Task CreateAsync(T model);
        public Task UpdateAsync(T model);
        public Task RemoveAsync(string id);
    }
}
