

using Pizza.Application.Core.Interfaces;
using Pizza.Data.Models.DTOS;
using Pizza.Data.Models.Entities;
using Pizza.Infrastructure.Repository.Interfaces;

namespace Pizza.Application.Core.Services
{
    public class DishService : IDishService

    {
        
        private readonly IDishRepo _dishRepo;

        public DishService(IDishRepo dishRepo)
        {
            _dishRepo = dishRepo;
        }

        public async Task<List<Dish>> GetAllDishes()
        {
            return await _dishRepo.GetAlldishes();
        }

        public async Task AddDish(DishDto dishDto)
        {
            await _dishRepo.AddDishAsync(dishDto);
        }

        public async Task UpdateDish(DishDto updatedDishDto)
        {
            await _dishRepo.UpdateDish(updatedDishDto);
        }

        public async Task DeleteDish(int dishId)
        {
            await _dishRepo.DeleteDish(dishId);
        }
    
}
}
