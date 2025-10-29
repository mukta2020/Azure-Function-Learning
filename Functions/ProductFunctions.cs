using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Text.Json;
using FunctionApp.Services;
using FunctionApp.Models;
using System.Threading.Tasks;
using System;

namespace FunctionApp.Functions;

public class ProductFunctions
{
    private readonly ProductService _service;

    public ProductFunctions(ProductService service)
    {
        _service = service;
    }

    [Function("GetProducts")]
    public async Task<HttpResponseData> GetProducts([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequestData req)
    {
        var list = await _service.GetAllAsync();
        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteAsJsonAsync(list);
        return res;
    }

    [Function("GetProductById")]
    public async Task<HttpResponseData> GetProductById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")] HttpRequestData req, string id)
    {
        if (!Guid.TryParse(id, out var gid)) return req.CreateResponse(HttpStatusCode.BadRequest);
        var p = await _service.GetAsync(gid);
        if (p == null) return req.CreateResponse(HttpStatusCode.NotFound);
        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteAsJsonAsync(p);
        return res;
    }

    [Function("CreateProduct")]
    public async Task<HttpResponseData> CreateProduct([HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequestData req)
    {
        var p = await JsonSerializer.DeserializeAsync<Product>(req.Body) ?? new Product();
        await _service.CreateAsync(p);
        var res = req.CreateResponse(HttpStatusCode.Created);
        await res.WriteAsJsonAsync(p);
        return res;
    }

    [Function("UpdateProduct")]
    public async Task<HttpResponseData> UpdateProduct([HttpTrigger(AuthorizationLevel.Function, "put", Route = "products/{id}")] HttpRequestData req, string id)
    {
        if (!Guid.TryParse(id, out var gid)) return req.CreateResponse(HttpStatusCode.BadRequest);
        var updated = await JsonSerializer.DeserializeAsync<Product>(req.Body) ?? new Product();
        var p = updated with { Id = gid };
        var ok = await _service.UpdateAsync(p);
        return req.CreateResponse(ok ? HttpStatusCode.NoContent : HttpStatusCode.NotFound);
    }

    [Function("DeleteProduct")]
    public async Task<HttpResponseData> DeleteProduct([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/{id}")] HttpRequestData req, string id)
    {
        if (!Guid.TryParse(id, out var gid)) return req.CreateResponse(HttpStatusCode.BadRequest);
        var ok = await _service.DeleteAsync(gid);
        return req.CreateResponse(ok ? HttpStatusCode.NoContent : HttpStatusCode.NotFound);
    }
}
