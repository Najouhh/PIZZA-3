using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using Pizza.Data;
using Pizza.Data.Models.DTOS.Order;
using Pizza.Data.Models.Entities;
using Pizza.Infrastructure.Repository.Interfaces;
using pizzariaV1.Data.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaTest
{
    [TestFixture]
    public class OrderServiceTest
    {
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<IOrderRepo> _mockOrderRepo;
        private Mock<IDishRepo> _mockDishRepo;
        private Mock<IMapper> _mockMapper;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            _mockOrderRepo = new Mock<IOrderRepo>();
            _mockDishRepo = new Mock<IDishRepo>();
            _mockMapper = new Mock<IMapper>();

            _orderService = new OrderService(
                _mockDishRepo.Object,
                _mockOrderRepo.Object,
                _mockMapper.Object,
                _mockUserManager.Object
            );
        }

        [Test]
        public async Task PlaceOrderAsync_ShouldApplyDiscount_ForPremiumUserWithThreeOrMoreDishes()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                DishQuantities = new List<int> { 3, 2, 1 },
                DishIds = new List<int> { 1, 2, 3 }
            };
            var dishes = new List<Dish>
            {
                new Dish { DishID = 1, Price = 10 },
                new Dish { DishID = 2, Price = 15 },
                new Dish { DishID = 3, Price = 20 }
            };
            var user = new ApplicationUser();

            _mockDishRepo.Setup(repo => repo.GetDishByID(It.IsAny<int>()))
                .ReturnsAsync((int id) => dishes.FirstOrDefault(d => d.DishID == id));
            _mockUserManager.Setup(um => um.FindByIdAsync("user-id")).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Premium" });  // Setup Premium Role

            // Act
            var result = await _orderService.PlaceOrderAsync(orderDto, "user-id");

            // Assert
            Assert.IsTrue(result);  // Check that the order was successfully placed

            // Verify that AddOrder was called exactly once
            _mockOrderRepo.Verify(repo => repo.AddOrder(It.IsAny<Order>()), Times.Once);

            // Verify that the discount was applied by checking the total price
            var orderArgument = (Order)_mockOrderRepo.Invocations.First().Arguments.First();
            Assert.AreEqual(64, orderArgument.TotalPrice);  // Expected total price after 20% discount
        }

        [Test]
        public async Task PlaceOrderAsync_ShouldNotApplyDiscount_ForRegularUserWithThreeOrMoreDishes()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                DishQuantities = new List<int> { 3, 2, 1 },
                DishIds = new List<int> { 1, 2, 3 }
            };
            var dishes = new List<Dish>
            {
                new Dish { DishID = 1, Price = 10 },
                new Dish { DishID = 2, Price = 15 },
                new Dish { DishID = 3, Price = 20 }
            };
            var user = new ApplicationUser();

            _mockDishRepo.Setup(repo => repo.GetDishByID(It.IsAny<int>()))
                .ReturnsAsync((int id) => dishes.FirstOrDefault(d => d.DishID == id));
            _mockUserManager.Setup(um => um.FindByIdAsync("user-id")).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Regular" });  // Setup Regular Role

            // Act
            var result = await _orderService.PlaceOrderAsync(orderDto, "user-id");

            // Assert
            Assert.IsTrue(result);  // Check that the order was successfully placed

            // Verify that AddOrder was called exactly once
            _mockOrderRepo.Verify(repo => repo.AddOrder(It.IsAny<Order>()), Times.Once);

            // Verify that the discount was NOT applied by checking the total price
            var orderArgument = (Order)_mockOrderRepo.Invocations.First().Arguments.First();
            Assert.AreEqual(80, orderArgument.TotalPrice);  // Expected total price without discount
        }

        [Test]
        public async Task PlaceOrderAsync_ShouldReturnFalse_WhenNoDishesFound()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                DishQuantities = new List<int> { 1, 2, 1 },
                DishIds = new List<int> { 1, 2, 3 }
            };

            _mockDishRepo.Setup(repo => repo.GetDishByID(It.IsAny<int>())).ReturnsAsync((Dish)null);

            // Act
            var result = await _orderService.PlaceOrderAsync(orderDto, "user-id");

            // Assert
            Assert.IsFalse(result);  // Expect false because no dishes were found
        }

        [Test]
        public async Task PlaceOrderAsync_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                DishQuantities = new List<int> { 1, 1, 1 },
                DishIds = new List<int> { 1, 2, 3 }
            };

            _mockDishRepo.Setup(repo => repo.GetDishByID(It.IsAny<int>())).ReturnsAsync(new Dish());
            _mockUserManager.Setup(um => um.FindByIdAsync("user-id")).ReturnsAsync((ApplicationUser)null);  // User not found

            // Act
            var result = await _orderService.PlaceOrderAsync(orderDto, "user-id");

            // Assert
            Assert.IsFalse(result);  // Expect false because user was not found
        }
    }
}
