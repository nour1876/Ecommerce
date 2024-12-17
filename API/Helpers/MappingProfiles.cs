using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using System.Linq.Expressions;

namespace API.Helpers
{
    public class MappingProfiles : Profile 
    {
        public MappingProfiles() {
         CreateMap<Product,ProductToReturnDto>()
                // the map doesn t understand how to convert from type(ProductBrand and ProductType )to string
                .ForMember(d=>d.ProductBrand,o=>o.MapFrom(s=>s.ProductBrand.Name)) //here for member ProductBrand take the name and affect it to d (dto)
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
        }
    }
}
