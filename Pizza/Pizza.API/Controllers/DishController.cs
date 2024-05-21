using Microsoft.AspNetCore.Mvc;
using Pizza.Application.Core.Interfaces;
using Pizza.Data.Models.DTOS;


namespace Pizza.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DishController : Controller
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDishes()
        {
            var dishes = await _dishService.GetAllDishes();
            return Ok(dishes);
        }
        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDish(DishDto dish)
        {
            await _dishService.UpdateDish(dish);
            return Ok("Dish has been updated");

        }
        [HttpPost("add")]
        public async Task<IActionResult> AddDishAsync(DishDto dishDto)
        {


            // Call the repository method to add the dish
            await _dishService.AddDish(dishDto);

            // Return success response
            return Ok("Dish added successfully!");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDish(int DishID)
        {
            await _dishService.DeleteDish(DishID);
            return Ok("Dish has been deleted");
        }
    }

}
