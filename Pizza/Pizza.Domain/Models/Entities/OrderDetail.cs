using pizzariaV1.Data.Models.Entities;

namespace Pizza.Data.Models.Entities
{
    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int DishID { get; set; }
        public int Quantity { get; set; }
        public virtual Dish Dish { get; set; }
        public virtual Order   Order { get; set; }
    }
}
