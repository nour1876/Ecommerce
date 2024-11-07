
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Helpers;

namespace API.Controllers
{

    public class ProductsController : BaseApiController
    {
        private StoreContext _context;
        private readonly IGenericRepository<Product> _repo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> repo, IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo,IMapper mapper , StoreContext context )
        {
            _context = context;
            _repo = repo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
        {
            if (productParams == null)
            {
                productParams = new ProductSpecParams(); // Default values for pagination (PageIndex=1, PageSize=6)
            }
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);

            var totalItems = await _repo.CountAsync(countSpec);
            var products = await _repo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,
                productParams.PageSize, totalItems, data));
        }



        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec= new ProductsWithTypesAndBrandsSpecification(id);
            var product= await _repo.GetEntityWithSpec(spec);
            if (product == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<Product,ProductToReturnDto>(product);
           
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            // 1. Validate the input data
            if (product== null)
            {
                return BadRequest(new ApiResponse(400, "Product data is required"));
            }



            // 3. Save the product to the repository
            _repo.Add(product);

            // 4. Commit the changes to the database
            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return BadRequest(new ApiResponse(400, "Problem adding product"));
            }

            // 5. Return the created product as DTO
            var productToReturn = _mapper.Map<ProductToReturnDto>(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productToReturn);
        }

        [HttpPost("Brand")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductBrand>> AddProductBrand(ProductBrand Brand)
        {
            // 1. Validate the input data
            if (Brand == null)
            {
                return BadRequest(new ApiResponse(400, "Product data is required"));
            }

            // 2. Map the DTO to the Product entity
            var brand = _mapper.Map<ProductBrand>(Brand);

            // 3. Save the product to the repository
            _productBrandRepo.Add(brand);

            // 4. Commit the changes to the database
            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return BadRequest(new ApiResponse(400, "Problem adding brand"));
            }

            // 5. Return the created product as DTO
            var brandtoreturn = _mapper.Map<ProductBrand>(brand);
            return StatusCode(StatusCodes.Status201Created, brandtoreturn);
        }
        [HttpPost("Type")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductType>> AddProductType(ProductType Type)
        {
            // 1. Validate the input data
            if (Type == null)
            {
                return BadRequest(new ApiResponse(400, "Product data is required"));
            }

            // 2. Map the DTO to the Product entity
            var type = _mapper.Map<ProductType>(Type);

            // 3. Save the product to the repository
            _productTypeRepo.Add(type);

            // 4. Commit the changes to the database
            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return BadRequest(new ApiResponse(400, "Problem adding type"));
            }

            // 5. Return the created product as DTO
            var typetoreturn = _mapper.Map<ProductType>(type);
            return StatusCode(StatusCodes.Status201Created, typetoreturn);
        }


    }
}
