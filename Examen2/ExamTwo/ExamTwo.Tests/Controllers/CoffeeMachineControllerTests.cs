using Xunit;
using Microsoft.AspNetCore.Mvc;
using ExamTwo.Models;
using ExamTwo.Controllers;
using System.Collections.Generic;

namespace ExamTwo.Tests.Controllers
{
    public class CoffeeMachineControllerTests
    {
        // Simple mock service for testing
        private class MockCoffeeMachineService : ExamTwo.Services.Interfaces.ICoffeeMachineService
        {
            public Dictionary<string, int> GetAvailableCoffees() => new Dictionary<string, int>
            {
                { "Americano", 10 }
            };
            
            public Dictionary<string, int> GetCoffeePrices() => new Dictionary<string, int>
            {
                { "Americano", 950 }
            };
            
            public Dictionary<string, int> GetCoffeeQuantities() => GetAvailableCoffees();
            
            public bool ValidateOrder(OrderRequest request, out string errorMessage)
            {
                errorMessage = "";
                return true;
            }
            
            public bool ValidatePayment(OrderRequest request, int cost, out string errorMessage)
            {
                errorMessage = "";
                return true;
            }
            
            public CoffeeMachineResponse ProcessPurchase(OrderRequest request)
            {
                return new CoffeeMachineResponse
                {
                    Success = true,
                    Message = "Successful purchase.",
                    ChangeAmount = 50,
                    ChangeBreakdown = new Dictionary<int, int> { { 50, 1 } }
                };
            }
            
            public bool HasEnoughChange(int changeAmount) => true;
        }

        [Fact]
        public void PurchaseCoffee_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Arrange
            var controller = new CoffeeMachineController(new MockCoffeeMachineService());

            // Act
            var result = controller.PurchaseCoffee(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request.", badRequest.Value);
        }

        [Fact]
        public void PurchaseCoffee_ShouldReturnOk_WhenPurchaseIsSuccessful()
        {
            // Arrange
            var controller = new CoffeeMachineController(new MockCoffeeMachineService());
            var orderRequest = new OrderRequest
            {
                Order = new Dictionary<string, int> { { "Americano", 1 } },
                Payment = new Payment { TotalAmount = 1000 }
            };

            // Act
            var result = controller.PurchaseCoffee(orderRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as string;
            Assert.Contains("Your change is 50 colones.", response);
            Assert.Contains("Breakdown:", response);
        }

        [Fact]
        public void PurchaseCoffee_ShouldReturnBadRequest_WhenServiceFails()
        {
            // Arrange
            var failingService = new FailingCoffeeMachineService();
            var controller = new CoffeeMachineController(failingService);
            var orderRequest = new OrderRequest
            {
                Order = new Dictionary<string, int> { { "Americano", 1 } },
                Payment = new Payment { TotalAmount = 1000 }
            };

            // Act
            var result = controller.PurchaseCoffee(orderRequest);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Simulated failed purchase", badRequest.Value);
        }

        [Fact]
        public void CheckCoffeeAvailability_ShouldReturnBadRequest_WhenOrderIsNull()
        {
            // Arrange
            var controller = new CoffeeMachineController(new MockCoffeeMachineService());

            // Act
            var result = controller.CheckCoffeeAvailability(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The order is empty.", badRequest.Value);
        }

        [Fact]
        public void CheckCoffeeAvailability_ShouldReturnOk_WhenOrderIsValid()
        {
            // Arrange
            var controller = new CoffeeMachineController(new MockCoffeeMachineService());
            var validOrder = new Dictionary<string, int> { { "Americano", 1 } };

            // Act
            var result = controller.CheckCoffeeAvailability(validOrder);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Confirmed availability. You can continue with the purchase.", okResult.Value);
        }

        [Fact]
        public void GetAvailableCoffees_ShouldReturnOk_WhenServiceReturnsData()
        {
            // Arrange
            var controller = new CoffeeMachineController(new MockCoffeeMachineService());

            // Act
            var result = controller.GetAvailableCoffees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var coffees = okResult.Value as Dictionary<string, int>;
            Assert.NotNull(coffees);
            Assert.Contains("Americano", coffees.Keys);
        }

        [Fact]
        public void GetCoffeePrices_ShouldReturnOk_WhenServiceReturnsData()
        {
            // Arrange
            var controller = new CoffeeMachineController(new MockCoffeeMachineService());

            // Act
            var result = controller.GetCoffeePrices();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var prices = okResult.Value as Dictionary<string, int>;
            Assert.NotNull(prices);
            Assert.Contains("Americano", prices.Keys);
        }

        [Fact]
        public void GetCoffeeQuantities_ShouldReturnOk_WhenServiceReturnsData()
        {
            // Arrange
            var controller = new CoffeeMachineController(new MockCoffeeMachineService());

            // Act
            var result = controller.GetCoffeeQuantities();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var quantities = okResult.Value as Dictionary<string, int>;
            Assert.NotNull(quantities);
            Assert.Contains("Americano", quantities.Keys);
        }

        // Failed service for testing
        private class FailingCoffeeMachineService : ExamTwo.Services.Interfaces.ICoffeeMachineService
        {
            public Dictionary<string, int> GetAvailableCoffees() => new Dictionary<string, int>();
            public Dictionary<string, int> GetCoffeePrices() => new Dictionary<string, int>();
            public Dictionary<string, int> GetCoffeeQuantities() => new Dictionary<string, int>();
            public bool ValidateOrder(OrderRequest request, out string errorMessage)
            {
                errorMessage = "";
                return true;
            }
            public bool ValidatePayment(OrderRequest request, int cost, out string errorMessage)
            {
                errorMessage = "";
                return true;
            }
            public CoffeeMachineResponse ProcessPurchase(OrderRequest request)
            {
                return new CoffeeMachineResponse
                {
                    Success = false,
                    Message = "Simulated failed purchase"
                };
            }
            public bool HasEnoughChange(int changeAmount) => true;
        }
    }
}