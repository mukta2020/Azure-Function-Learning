using FunctionApp.Models;
using FunctionApp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionApp.Services;

public class ProductService
{
    private readonly IProductRepository _repo;
    public ProductService(IProductRepository repo) => _repo = repo;

    public Task<IEnumerable<Product>> GetAllAsync() => _repo.GetAllAsync();
    public Task<Product?> GetAsync(Guid id) => _repo.GetAsync(id);
    public Task CreateAsync(Product p) => _repo.CreateAsync(p);
    public Task<bool> UpdateAsync(Product p) => _repo.UpdateAsync(p);
    public Task<bool> DeleteAsync(Guid id) => _repo.DeleteAsync(id);
}
