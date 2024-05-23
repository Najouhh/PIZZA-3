

using AutoMapper;
using Microsoft.Extensions.Logging;
using Pizza.Application.Core.Interfaces;
using Pizza.Data.Models.DTOS;
using Pizza.Data.Models.Entities;
using Pizza.Infrastructure.Repository.Interfaces;

namespace Pizza.Application.Core.Services
{
    public class DishService : IDishService

    {
        
        private readonly IDishRepo _dishRepo;
        private readonly ILogger<DishService> _logger;
        private readonly IMapper _mapper;

        public DishService(IDishRepo dishRepo, ILogger<DishService> logger, IMapper mapper)
        {
            _dishRepo = dishRepo;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<Dish>> GetAllDishes()
        {
            return await _dishRepo.GetAlldishes();
        }
        public async Task<bool> AddDish(DishDto dishDto)
        {
            var ingredientIdsList = dishDto.IngredientIDs.ToList(); // Convert ICollection<int> to List<int>
            var ingredients = await _dishRepo.GetIngredientsByIds(ingredientIdsList);
            var missingIngredientIds = dishDto.IngredientIDs.Except(ingredients.Select(i => i.IngredientID)).ToList();

            if (missingIngredientIds.Any())
            {
                var errorMessage = $"The following ingredient IDs do not exist: {string.Join(", ", missingIngredientIds)}";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            var category = await _dishRepo.GetCategoryById(dishDto.CategoryID);
            if (category == null)
            {
                var errorMessage = $"The category ID {dishDto.CategoryID} does not exist.";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            var dish = _mapper.Map<Dish>(dishDto);
            dish.Category = category;
            dish.Ingredients = ingredients;

            await _dishRepo.AddDish(dish);
            return true;
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
      

        public async Task UpdateDish(DishDto updatedDish)
        {
            var original = await _dishRepo.GetDishByID(updatedDish.DishID);
            if (original == null)
            {
                _logger.LogError("Dish with ID {DishID} not found.", updatedDish.DishID);
                throw new ArgumentException("Dish not found.");
            }

            // Map updatedDish to original entity
            _mapper.Map(updatedDish, original);

            // Update Category
            var category = await _dishRepo.GetCategoryById(updatedDish.CategoryID);
            if (category == null)
            {
                _logger.LogError("Category with ID {CategoryID} not found.", updatedDish.CategoryID);
                throw new ArgumentException("Category not found.");
            }
            original.Category = category;

            // Update Ingredients
            original.Ingredients.Clear();
            foreach (var ingredientId in updatedDish.IngredientIDs)
            {
                var ingredients = await _dishRepo.GetIngredientsByIds(new List<int> { ingredientId });
                if (ingredients.Count == 0)
                {
                    _logger.LogError("Ingredient with ID {IngredientID} not found.", ingredientId);
                    throw new ArgumentException($"Ingredient with ID {ingredientId} not found.");
                }

                foreach (var ingredient in ingredients)
                {
                    original.Ingredients.Add(ingredient); 
                }
            }

            await _dishRepo.UpdateDish(original); 
        }



        public async Task DeleteDish(int dishId)
        {
            await _dishRepo.DeleteDish(dishId);
        }
    
}
}
