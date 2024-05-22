using Pizza.Data.Models.DTOS;
using Pizza.Data.Models.Entities;

namespace Pizza.Infrastructure.Repository.Interfaces
{
    public interface IDishRepo
    {
        Task AddDish(DishDto dishDTO);
        Task AddCategory(Category category);
        Task AddIngredients(Ingredient ingredient);
        Task<List<Dish>> GetAlldishes();
        Task<List<Category>> GetAllCategories();
        Task<List<Ingredient>> GetAllIngredients();
        Task UpdateDish(DishDto updatedDish);

        Task DeleteDish(int DishID);
        Task<Dish> GetByIdAsync(int id);
    }
}
