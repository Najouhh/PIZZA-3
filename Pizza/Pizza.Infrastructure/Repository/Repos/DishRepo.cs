using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pizza.Data.Models.DTOS;
using Pizza.Data.Models.Entities;
using Pizza.Infrastructure.Data;
using Pizza.Infrastructure.Repository.Interfaces;

namespace Pizza.Infrastructure.Repository.Repos
{
    public class DishRepo : IDishRepo
    {
        private readonly PizzaContext _context;
        private readonly IMapper _mapper;



        public DishRepo(PizzaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }


        public async Task<List<Dish>> GetAlldishes()
        {
            return await _context.Dishes
          .Include(d => d.Category)
           .Include(d => d.Ingredients)

         .ToListAsync();
        }

        public async Task AddDishAsync(DishDto viewModel)
        {
            var ingredientIds = viewModel.IngredientIDs;
            var categoryId = viewModel.CategoryID;

            // Fetch selected ingredients from the database asynchronously
            var ingredients = await _context.Ingredients.Where(i => ingredientIds.Contains(i.IngredientID)).ToListAsync();

            // Fetch the category from the database
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryID == categoryId);

            if (category == null)
            {
                // Handle error: invalid category ID
                return;
            }

            // Map DishDto to Dish using AutoMapper
            var dish = _mapper.Map<Dish>(viewModel);

            // Assign the fetched category and ingredients
            dish.Category = category;
            dish.Ingredients = ingredients;

            // Add the Dish instance to the context
            _context.Dishes.Add(dish);

            // Save changes to the database asynchronously
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDish(DishDto updatedDish)
        {
            var original = await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Ingredients)
                .FirstOrDefaultAsync(x => x.DishID == updatedDish.DishID);

            if (original != null)
            {
                // Update DishName and Price
                original.DishName = updatedDish.DishName;
                original.Price = updatedDish.Price;

                // Update CategoryID using DbContext.Entry
                _context.Entry(original).Property("CategoryID").CurrentValue = updatedDish.CategoryID;

                // Update Ingredients
                original.Ingredients.Clear();
                foreach (var ingredientId in updatedDish.IngredientIDs)
                {
                    var ingredient = await _context.Ingredients.FindAsync(ingredientId);
                    if (ingredient != null)
                    {
                        original.Ingredients.Add(ingredient);
                    }
                }

                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteDish(int DishID)
        {
            var dish = _context.Dishes.FirstOrDefault(x => x.DishID == DishID);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();

        }
        public async Task<Dish> GetByIdAsync(int id)
        {
            return await _context.Dishes.FindAsync(id);
        }

    }
}
