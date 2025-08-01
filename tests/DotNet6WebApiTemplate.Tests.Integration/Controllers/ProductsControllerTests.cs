using DotNet6WebApiTemplate.Api;
using DotNet6WebApiTemplate.Application.DTOs;
using DotNet6WebApiTemplate.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace DotNet6WebApiTemplate.Tests.Integration.Controllers;

public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ProductsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing ApplicationDbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add a in-memory database for testing
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Ensure the database is created and seeded
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_ShouldReturnAllProducts()
    {
        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer.Deserialize<List<ProductDto>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        products.Should().NotBeNull();
        products.Should().HaveCountGreaterOrEqualTo(2); // Seeded data
    }

    [Fact]
    public async Task GetProduct_WithValidId_ShouldReturnProduct()
    {
        // Act
        var response = await _client.GetAsync("/api/products/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var product = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        product.Should().NotBeNull();
        product!.Id.Should().Be(1);
        product.Name.Should().Be("Sample Product 1");
    }

    [Fact]
    public async Task GetProduct_WithInvalidId_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/products/999");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateProduct_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        var createProductDto = new CreateProductDto
        {
            Name = "Integration Test Product",
            Description = "Test Description",
            Price = 99.99m,
            Quantity = 10,
            Category = "Test Category"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", createProductDto);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var content = await response.Content.ReadAsStringAsync();
        var createdProduct = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        createdProduct.Should().NotBeNull();
        createdProduct!.Name.Should().Be(createProductDto.Name);
        createdProduct.Description.Should().Be(createProductDto.Description);
        createdProduct.Price.Should().Be(createProductDto.Price);
    }

    [Fact]
    public async Task UpdateProduct_WithValidData_ShouldUpdateProduct()
    {
        // Arrange
        var updateProductDto = new UpdateProductDto
        {
            Name = "Updated Product Name",
            Description = "Updated Description",
            Price = 149.99m,
            Quantity = 20,
            Category = "Updated Category"
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/products/1", updateProductDto);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var updatedProduct = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        updatedProduct.Should().NotBeNull();
        updatedProduct!.Name.Should().Be(updateProductDto.Name);
        updatedProduct.Price.Should().Be(updateProductDto.Price);
    }

    [Fact]
    public async Task DeleteProduct_WithValidId_ShouldDeleteProduct()
    {
        // First, verify the product exists
        var getResponse = await _client.GetAsync("/api/products/2");
        getResponse.EnsureSuccessStatusCode();

        // Act
        var deleteResponse = await _client.DeleteAsync("/api/products/2");

        // Assert
        deleteResponse.EnsureSuccessStatusCode();
        deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        // Verify the product is deleted (should return 404)
        var verifyResponse = await _client.GetAsync("/api/products/2");
        verifyResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}