using FunctionApp.Models;
using FunctionApp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionApp.Services;

public class CustomerService
{
    private readonly ICustomerRepository _repo;
    public CustomerService(ICustomerRepository repo) => _repo = repo;

    public Task<IEnumerable<Customer>> GetAllAsync() => _repo.GetAllAsync();
    public Task<Customer?> GetAsync(Guid id) => _repo.GetAsync(id);
    public Task CreateAsync(Customer c) => _repo.CreateAsync(c);
    public Task<bool> UpdateAsync(Customer c) => _repo.UpdateAsync(c);
    public Task<bool> DeleteAsync(Guid id) => _repo.DeleteAsync(id);
}
