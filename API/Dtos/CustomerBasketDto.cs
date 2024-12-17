using Core.Entities;

namespace API.Dtos
{
    public class CustomerBasketDto
    {
        public int Id { get; set; }
        public List<BasketItem> Items { get; set; }

    }
}
