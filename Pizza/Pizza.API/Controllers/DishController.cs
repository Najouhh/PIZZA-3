using Microsoft.AspNetCore.Mvc;
using Pizza.Application.Core.Interfaces;
using Pizza.Data.Models.DTOS;
using Pizza.Data.Models.Entities;
using System.ComponentModel;


namespace Pizza.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DishController : Controller
    {
        private readonly IDishService _dishService;
        private readonly ILogger<DishController> _logger;

        public DishController(IDishService dishService, ILogger<DishController> logger)
        {
            _dishService = dishService;
            _logger = logger;
        }

        [HttpGet("Get All Dishes")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDishes()
        {
            try
            {
                _logger.LogInformation("Fetching all dishes.");
                var dishes = await _dishService.GetAllDishes();
                return Ok(dishes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all dishes.");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("Get All Categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _dishService.GetAllCategories();
            return Ok(categories);
        }
        [HttpGet("Get All Ingredients")]
        public async Task<IActionResult> GetAllIngredients()
        {
            var Ingredients = await _dishService.GetAllIngredients();
            return Ok(Ingredients);
        }
        [HttpPut("Update a Dish")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDish(DishDto dish)
        {
            await _dishService.UpdateDish(dish);
            return Ok("Dish has been updated");

        }
        [HttpPost("add a new dish")]
        public async Task<IActionResult> AddDish(DishDto dishDto)
        {
            await _dishService.AddDish(dishDto);
            return Ok("Dish added successfully!");
        }
        [HttpPost("add a new Category")]
        public async Task<IActionResult> AddCategory(Category category)
        {
            await _dishService.AddCategory(category);
            return Ok("Category added successfully!");
        }
        [HttpPost("add a new Ingredient")]
        public async Task<IActionResult> AddIngredients(Ingredient ingredient)
        {
            await _dishService.AddIngredients(ingredient);
            return Ok("Category added successfully!");
        }
        [HttpDelete("Delete A dish")]
        public async Task<IActionResult> DeleteDish(int DishID)
        {
            await _dishService.DeleteDish(DishID);
            return Ok("Dish has been deleted");
        }
    }

}
