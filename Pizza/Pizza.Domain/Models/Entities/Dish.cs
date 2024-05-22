using pizzariaV1.Data.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Pizza.Data.Models.Entities
{
    public class Dish
    {
        [Key]
        
        public int DishID { get; set; }
        [StringLength(50)]
        public string ?DishName { get; set;}
        [Range(0, 500)]
        public decimal? Price { get; set; }
       
        public virtual Category? Category { get; set; }
        public virtual ICollection<Ingredient> ?Ingredients { get; set;}
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
