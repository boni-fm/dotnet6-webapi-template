using AutoMapper;
using WebApi.Base.Interfaces;
using WebApi.Services.Data.Entities;
using WebApi.Services.DTOs;
using WebApi.Services.Repositories;

namespace WebApi.Services.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
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
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        var product = _mapper.Map<Product>(createProductDto);
        var createdProduct = await _productRepository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        
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
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<ProductDto>(updatedProduct);
    }

    public async Task DeleteProductAsync(int id)
    {
        var exists = await _productRepository.ExistsAsync(id);
        if (!exists)
        {
            throw new ArgumentException($"Product with ID {id} not found.");
        }

        await _productRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category)
    {
        var products = await _productRepository.GetByCategoryAsync(category);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
    {
        var products = await _productRepository.GetActiveProductsAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}