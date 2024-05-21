namespace Pizza.Data.Models.DTOS.Order
{
    public class OrderDto
    {
        public List<int> DishIds { get; set; } // IDs of the dishes being ordered
        public List<int> DishQuantities { get; set; }

    }
}
