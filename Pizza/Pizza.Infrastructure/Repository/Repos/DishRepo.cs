using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pizza.Data.Models.DTOS;
using Pizza.Data.Models.Entities;
using Pizza.Infrastructure.Data;
using Pizza.Infrastructure.Repository.Interfaces;

namespace Pizza.Infrastructure.Repository.Repos
{
    public class DishRepo : IDishRepo
    {
        private readonly PizzaContext _context;
        private readonly ILogger<DishRepo> _logger;
        private readonly IMapper _mapper;

        public DishRepo(PizzaContext context, ILogger<DishRepo> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<Dish>> GetAlldishes()
        {
            return await _context.Dishes
           .Include(d => d.Category)
           .Include(d => d.Ingredients)
           .ToListAsync();
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<List<Ingredient>> GetAllIngredients()
        {
            return await _context.Ingredients.ToListAsync();
        }
        public async Task AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        public async Task AddIngredients(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Ingredient>> GetIngredientsByIds(List<int> ingredientIds)
        {
            return await _context.Ingredients.Where(i => ingredientIds.Contains(i.IngredientID)).ToListAsync();
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            return await _context.Categories.SingleOrDefaultAsync(c => c.CategoryID == categoryId);
        }

        public async Task AddDish(Dish dish)
        {
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDish(int DishID)
        {
            var dish = _context.Dishes.FirstOrDefault(x => x.DishID == DishID);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateDish(DishDto updatedDish)
        //{
        //    var original = await _context.Dishes
        //        .Include(d => d.Category)
        //        .Include(d => d.Ingredients)
        //        .FirstOrDefaultAsync(x => x.DishID == updatedDish.DishID);

        //    if (original != null)
        //    {
        //        // uppdatera namnet och pris
        //        original.DishName = updatedDish.DishName;
        //        original.Price = updatedDish.Price;

        //        // Update category
        //        _context.Entry(original).Property("CategoryID").CurrentValue = updatedDish.CategoryID;

        //        // Update Ingredients
        //        original.Ingredients.Clear();
        //        foreach (var ingredientId in updatedDish.IngredientIDs)
        //        {
        //            var ingredient = await _context.Ingredients.FindAsync(ingredientId);
        //            if (ingredient != null)
        //            {
        //                original.Ingredients.Add(ingredient);
        //            }
        //        }

        //        await _context.SaveChangesAsync();
        //    }
        //}
        public async Task UpdateDish(Dish updatedDish)
        {
            _context.Entry(updatedDish).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Dish> GetDishByID(int DishId)
        {
            return await _context.Dishes
                      .Include(d => d.Category)
                      .Include(d => d.Ingredients)
                      .FirstOrDefaultAsync(x => x.DishID == DishId);
        }

    }
}
