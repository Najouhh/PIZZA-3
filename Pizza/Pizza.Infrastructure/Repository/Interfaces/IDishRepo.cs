using Pizza.Data.Models.DTOS;
using Pizza.Data.Models.Entities;
using System.Threading.Tasks;

namespace Pizza.Infrastructure.Repository.Interfaces
{
    public interface IDishRepo
    {
       
        Task<List<Ingredient>> GetIngredientsByIds(List<int> ingredientIds);
        Task<Category> GetCategoryById(int categoryId);
        Task AddDish(Dish dish);

        Task AddCategory(Category category);
        Task AddIngredients(Ingredient ingredient);
        Task<List<Dish>> GetAlldishes();
        Task<List<Category>> GetAllCategories();
        Task<List<Ingredient>> GetAllIngredients();
        //Task UpdateDish(DishDto updatedDish);
        Task UpdateDish(Dish updatedDish);
        Task DeleteDish(int DishID);
        Task<Dish> GetDishByID(int DishId);
    }
}
