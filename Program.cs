using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Worker.Configuration;
using FunctionApp.Repositories;
using FunctionApp.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
        s.AddSingleton<IProductRepository, InMemoryProductRepository>();
        s.AddScoped<CustomerService>();
        s.AddScoped<ProductService>();
    })
    .Build();

host.Run();
