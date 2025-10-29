using FunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionApp.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<Guid, Product> _store = new();

    public Task CreateAsync(Product product)
    {
        _store[product.Id] = product;
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(_store.Remove(id));
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Product>>(_store.Values.ToList());
    }

    public Task<Product?> GetAsync(Guid id)
    {
        _store.TryGetValue(id, out var p);
        return Task.FromResult(p);
    }

    public Task<bool> UpdateAsync(Product product)
    {
        if (!_store.ContainsKey(product.Id)) return Task.FromResult(false);
        _store[product.Id] = product;
        return Task.FromResult(true);
    }
}
