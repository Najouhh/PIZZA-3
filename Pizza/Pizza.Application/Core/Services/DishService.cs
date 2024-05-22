

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
            await _dishRepo.AddDish(dishDto);
        }
        public async Task AddCategory(Category category)
        {
            await _dishRepo.AddCategory(category);
        }
        public async Task AddIngredients(Ingredient ingredient)
        {
            await _dishRepo.AddIngredients(ingredient);
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await _dishRepo.GetAllCategories();
        }

       public async Task<List<Ingredient>> GetAllIngredients()
        {
            return await _dishRepo.GetAllIngredients();
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
