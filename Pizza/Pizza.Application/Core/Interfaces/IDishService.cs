using Pizza.Data.Models.DTOS;
using Pizza.Data.Models.Entities;

namespace Pizza.Application.Core.Interfaces
{
    public interface IDishService

    {
        Task<List<Dish>> GetAllDishes();
        Task AddDish(DishDto dishDto);
        Task UpdateDish(DishDto updatedDishDto);
        Task DeleteDish(int dishId);
    }
}
