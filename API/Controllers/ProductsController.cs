
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private StoreContext _context;
        private readonly IGenericRepository<Product> _repo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> repo, IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo,IMapper mapper )
        {
            _repo = repo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _repo.ListAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
                /*products.Select(product => new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            }).ToList();*/

        }

        private object Ok(Func<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>> map)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec= new ProductsWithTypesAndBrandsSpecification(id);
            var product= await _repo.GetEntityWithSpec(spec);
            return _mapper.Map<Product,ProductToReturnDto>(product);
                /*new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name

            };*/
        }
        [HttpGet("brands")]

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());

        }
        [HttpGet("types")]

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());

        }


    }
    }
