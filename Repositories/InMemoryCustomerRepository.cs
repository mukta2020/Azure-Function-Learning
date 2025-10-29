using FunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionApp.Repositories;

public class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly Dictionary<Guid, Customer> _store = new();

    public Task CreateAsync(Customer customer)
    {
        _store[customer.Id] = customer;
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(_store.Remove(id));
    }

    public Task<IEnumerable<Customer>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Customer>>(_store.Values.ToList());
    }

    public Task<Customer?> GetAsync(Guid id)
    {
        _store.TryGetValue(id, out var c);
        return Task.FromResult(c);
    }

    public Task<bool> UpdateAsync(Customer customer)
    {
        if (!_store.ContainsKey(customer.Id)) return Task.FromResult(false);
        _store[customer.Id] = customer;
        return Task.FromResult(true);
    }
}
