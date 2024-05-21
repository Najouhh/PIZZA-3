namespace Pizza.Data.Models.DTOS
{
    public class DishDto
    {
        public int DishID { get; set; }
        public string? DishName { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; } // Assuming you only need the CategoryID for updating
        public List<int> ?IngredientIDs { get; set; }
    }
}
