using Pizza.Data.Models.DTOS;
using Pizza.Data.Models.Entities;
using System.Threading.Tasks;

namespace Pizza.Application.Core.Interfaces
{
    public interface IDishService

    {
        Task<List<Dish>> GetAllDishes();
        Task<List<Category>> GetAllCategories();
        Task<List<Ingredient>> GetAllIngredients();
        Task<bool> AddDish(DishDto dishDto);
        Task AddCategory(Category category);
        Task AddIngredients(Ingredient ingredient);
        Task UpdateDish(DishDto updatedDishDto);
        Task DeleteDish(int dishId);
    }
}
