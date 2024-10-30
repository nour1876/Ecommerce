using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    // this class i done to resolve where to get the url of the picture in non static way(in our desktop as example)
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _config;
        public ProductUrlResolver(IConfiguration config
            ) { 
        _config = config;   
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                //Be attention it must be the same as appsettings ApiUrl
                return _config["ApiUrl"]+source.PictureUrl; // this will give the full path to the image
            }
            return null;
        }
    }
}
