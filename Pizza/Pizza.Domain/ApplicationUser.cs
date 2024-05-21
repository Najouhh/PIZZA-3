
using Microsoft.AspNetCore.Identity;
using pizzariaV1.Data.Models.Entities;

namespace Pizza.Data
{
    public class ApplicationUser : IdentityUser
    {
        
        public int UserPoints { get; set; }
       public ICollection<Order> Orders { get; set; }
     
    }
}
