using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pizza.Data.Models.Entities
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [StringLength(50)]
        public string ?CategoryName { get; set; }
        [JsonIgnore]
        public virtual ICollection<Dish> ?Dishes { get; set; }
    }
}
