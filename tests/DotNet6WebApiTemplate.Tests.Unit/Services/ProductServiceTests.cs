using DotNet6WebApiTemplate.Application.DTOs;
using DotNet6WebApiTemplate.Application.Services;
using DotNet6WebApiTemplate.Domain.Entities;
using DotNet6WebApiTemplate.Domain.Interfaces;
using AutoMapper;
using Moq;
using FluentAssertions;

namespace DotNet6WebApiTemplate.Tests.Unit.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _productService = new ProductService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 10.00m },
            new Product { Id = 2, Name = "Product 2", Price = 20.00m }
        };
        var productDtos = new List<ProductDto>
        {
            new ProductDto { Id = 1, Name = "Product 1", Price = 10.00m },
            new ProductDto { Id = 2, Name = "Product 2", Price = 20.00m }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
        _mockMapper.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(productDtos);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(productDtos);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithValidId_ShouldReturnProduct()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Name = "Test Product", Price = 15.00m };
        var productDto = new ProductDto { Id = productId, Name = "Test Product", Price = 15.00m };

        _mockRepository.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
        _mockMapper.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

        // Act
        var result = await _productService.GetProductByIdAsync(productId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(productDto);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var productId = 999;
        _mockRepository.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

        // Act
        var result = await _productService.GetProductByIdAsync(productId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateProductAsync_ShouldCreateAndReturnProduct()
    {
        // Arrange
        var createDto = new CreateProductDto { Name = "New Product", Price = 25.00m };
        var product = new Product { Name = "New Product", Price = 25.00m };
        var createdProduct = new Product { Id = 1, Name = "New Product", Price = 25.00m };
        var productDto = new ProductDto { Id = 1, Name = "New Product", Price = 25.00m };

        _mockMapper.Setup(m => m.Map<Product>(createDto)).Returns(product);
        _mockRepository.Setup(r => r.CreateAsync(product)).ReturnsAsync(createdProduct);
        _mockMapper.Setup(m => m.Map<ProductDto>(createdProduct)).Returns(productDto);

        // Act
        var result = await _productService.CreateProductAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(productDto);
        _mockRepository.Verify(r => r.CreateAsync(product), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_WithValidId_ShouldDeleteProduct()
    {
        // Arrange
        var productId = 1;
        _mockRepository.Setup(r => r.ExistsAsync(productId)).ReturnsAsync(true);

        // Act
        await _productService.DeleteProductAsync(productId);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(productId), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var productId = 999;
        _mockRepository.Setup(r => r.ExistsAsync(productId)).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.DeleteProductAsync(productId));
        exception.Message.Should().Contain($"Product with ID {productId} not found");
    }
}