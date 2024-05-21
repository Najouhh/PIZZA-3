using System.Text.Json.Serialization;

namespace Pizza.Data.Models.DTOS.User
{
    public class UserLogin
    {
    //    [JsonIgnore]
    //    public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        //[JsonIgnore]
        //public string RoleName {  get; set; }
    }
}
