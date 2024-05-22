namespace Pizza.Data.Models.DTOS
{
    public class DishDto
    {
        public int DishID { get; set; }
        public string? DishName { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; } 
        public ICollection<int>?IngredientIDs { get; set; }
    }
}
