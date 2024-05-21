using Pizza.Data;
using Pizza.Data.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pizzariaV1.Data.Models.Entities
{
    public class Order
    {

        [Key]
        public int OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }

        
        public virtual ApplicationUser ApplicationUser { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }


    }
}
