using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Text.Json;
using FunctionApp.Services;
using FunctionApp.Models;
using System.Threading.Tasks;
using System;

namespace FunctionApp.Functions;

public class CustomerFunctions
{
    private readonly CustomerService _service;

    public CustomerFunctions(CustomerService service)
    {
        _service = service;
    }

    [Function("GetCustomers")]
    public async Task<HttpResponseData> GetCustomers([HttpTrigger(AuthorizationLevel.Function, "get", Route = "customers")] HttpRequestData req)
    {
        var list = await _service.GetAllAsync();
        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteAsJsonAsync(list);
        return res;
    }

    [Function("GetCustomerById")]
    public async Task<HttpResponseData> GetCustomerById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "customers/{id}")] HttpRequestData req, string id)
    {
        if (!Guid.TryParse(id, out var gid)) return req.CreateResponse(HttpStatusCode.BadRequest);
        var c = await _service.GetAsync(gid);
        if (c == null) return req.CreateResponse(HttpStatusCode.NotFound);
        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteAsJsonAsync(c);
        return res;
    }

    [Function("CreateCustomer")]
    public async Task<HttpResponseData> CreateCustomer([HttpTrigger(AuthorizationLevel.Function, "post", Route = "customers")] HttpRequestData req)
    {
        var c = await JsonSerializer.DeserializeAsync<Customer>(req.Body) ?? new Customer();
        await _service.CreateAsync(c);
        var res = req.CreateResponse(HttpStatusCode.Created);
        await res.WriteAsJsonAsync(c);
        return res;
    }

    [Function("UpdateCustomer")]
    public async Task<HttpResponseData> UpdateCustomer([HttpTrigger(AuthorizationLevel.Function, "put", Route = "customers/{id}")] HttpRequestData req, string id)
    {
        if (!Guid.TryParse(id, out var gid)) return req.CreateResponse(HttpStatusCode.BadRequest);
        var updated = await JsonSerializer.DeserializeAsync<Customer>(req.Body) ?? new Customer();
        var c = updated with { Id = gid };
        var ok = await _service.UpdateAsync(c);
        return req.CreateResponse(ok ? HttpStatusCode.NoContent : HttpStatusCode.NotFound);
    }

    [Function("DeleteCustomer")]
    public async Task<HttpResponseData> DeleteCustomer([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "customers/{id}")] HttpRequestData req, string id)
    {
        if (!Guid.TryParse(id, out var gid)) return req.CreateResponse(HttpStatusCode.BadRequest);
        var ok = await _service.DeleteAsync(gid);
        return req.CreateResponse(ok ? HttpStatusCode.NoContent : HttpStatusCode.NotFound);
    }
}
