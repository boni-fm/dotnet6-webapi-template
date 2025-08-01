using AutoMapper;
using DotNet6WebApiTemplate.Application.DTOs;
using DotNet6WebApiTemplate.Application.Interfaces;
using DotNet6WebApiTemplate.Domain.Entities;
using DotNet6WebApiTemplate.Domain.Interfaces;

namespace DotNet6WebApiTemplate.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product != null ? _mapper.Map<ProductDto>(product) : null;
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        var product = _mapper.Map<Product>(createProductDto);
        var createdProduct = await _productRepository.CreateAsync(product);
        return _mapper.Map<ProductDto>(createdProduct);
    }

    public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            throw new ArgumentException($"Product with ID {id} not found.");
        }

        _mapper.Map(updateProductDto, existingProduct);
        var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
        return _mapper.Map<ProductDto>(updatedProduct);
    }

    public async Task DeleteProductAsync(int id)
    {
        if (!await _productRepository.ExistsAsync(id))
        {
            throw new ArgumentException($"Product with ID {id} not found.");
        }

        await _productRepository.DeleteAsync(id);
    }
}