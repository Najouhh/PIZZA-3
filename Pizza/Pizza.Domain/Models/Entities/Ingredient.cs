using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pizza.Data.Models.Entities
{
    public class Ingredient
    {
        [Key] 
    public int IngredientID { get; set; }
        [StringLength(50)]
    public string? IngredientName { get; set; }
        [JsonIgnore]
    public virtual ICollection<Dish> ?Dishes { get; set; }
    }
}
