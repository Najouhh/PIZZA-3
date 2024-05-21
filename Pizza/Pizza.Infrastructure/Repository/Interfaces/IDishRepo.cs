using Pizza.Data.Models.DTOS;
using Pizza.Data.Models.Entities;

namespace Pizza.Infrastructure.Repository.Interfaces
{
    public interface IDishRepo
    {
        Task AddDishAsync(DishDto dishDTO);

        Task<List<Dish>> GetAlldishes();
        Task UpdateDish(DishDto updatedDish);

        Task DeleteDish(int DishID);
        Task<Dish> GetByIdAsync(int id);
    }
}
