using Xunit;
using ExamTwo.Models;
using ExamTwo.Services;
using System.Collections.Generic;

namespace ExamTwo.Tests.Services
{
    public class CoffeeMachineServiceTests
    {
        // Mock repository for testing
        private class MockCoffeeMachineRepository : ExamTwo.Repositories.Interfaces.ICoffeeMachineRepository
        {
            public List<Coffee> GetAllCoffees() => new List<Coffee>
            {
                new Coffee("Americano", 950, 10),
                new Coffee("Cappuccino", 1200, 8)
            };
            
            public Coffee GetCoffeeByName(string name)
            {
                if (name == "Americano")
                    return new Coffee("Americano", 950, 10);
                return null;
            }

            public void UpdateCoffeeQuantity(string name, int quantity) { }
            
            public List<Coin> GetAllCoins() => new List<Coin>
            {
                new Coin(500, 20),
                new Coin(100, 30),
                new Coin(50, 50)
            };
            
            public Coin GetCoinByDenomination(int denomination)
            {
                if (denomination == 500) return new Coin(500, 20);
                if (denomination == 100) return new Coin(100, 30);
                if (denomination == 50) return new Coin(50, 50);
                if (denomination == 25) return new Coin(25, 25);
                return null;
            }
                
            public void UpdateCoinQuantity(int denomination, int quantity) { }
            
            public void InitializeMachine() { }
        }

        [Fact]
        public void ValidateOrder_ShouldReturnTrue_WhenOrderIsValid()
        {
            // Arrange
            var service = new CoffeeMachineService(new MockCoffeeMachineRepository());
            var orderRequest = new OrderRequest
            {
                Order = new Dictionary<string, int> { { "Americano", 2 } },
                Payment = new Payment { TotalAmount = 2000 }
            };

            // Act
            var isValid = service.ValidateOrder(orderRequest, out string errorMessage);

            // Assert
            Assert.True(isValid);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void ValidateOrder_ShouldReturnFalse_WhenOrderIsNull()
        {
            // Arrange
            var service = new CoffeeMachineService(new MockCoffeeMachineRepository());

            // Act
            var isValid = service.ValidateOrder(null, out string errorMessage);

            // Assert
            Assert.False(isValid);
            Assert.Equal("The order is empty.", errorMessage);
        }

        [Fact]
        public void ValidateOrder_ShouldReturnFalse_WhenCoffeeDoesNotExist()
        {
            // Arrange
            var service = new CoffeeMachineService(new MockCoffeeMachineRepository());
            var orderRequest = new OrderRequest
            {
                Order = new Dictionary<string, int> { { "Espresso", 1 } },
                Payment = new Payment { TotalAmount = 1000 }
            };

            // Act
            var isValid = service.ValidateOrder(orderRequest, out string errorMessage);

            // Assert
            Assert.False(isValid);
            Assert.Contains("Espresso", errorMessage);
        }

        [Fact]
        public void ValidatePayment_ShouldReturnTrue_WhenPaymentIsValid()
        {
            // Arrange
            var service = new CoffeeMachineService(new MockCoffeeMachineRepository());
            var orderRequest = new OrderRequest
            {
                Order = new Dictionary<string, int> { { "Americano", 1 } },
                Payment = new Payment
                {
                    TotalAmount = 1000,
                    Coins = new List<int> { 500, 500 },
                    Bills = new List<int>()
                }
            };

            // Act
            var isValid = service.ValidatePayment(orderRequest, 950, out string errorMessage);

            // Assert
            Assert.True(isValid);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public void ValidatePayment_ShouldReturnFalse_WhenPaymentIsNull()
        {
            // Arrange
            var service = new CoffeeMachineService(new MockCoffeeMachineRepository());
            var orderRequest = new OrderRequest
            {
                Order = new Dictionary<string, int> { { "Americano", 1 } },
                Payment = null
            };

            // Act
            var isValid = service.ValidatePayment(orderRequest, 950, out string errorMessage);

            // Assert
            Assert.False(isValid);
            Assert.Equal("Payment information is required.", errorMessage);
        }

        [Fact]
        public void ProcessPurchase_ShouldReturnSuccessResponse_WhenPurchaseIsValid()
        {
            // Arrange
            var repository = new MockCoffeeMachineRepository();
            var service = new CoffeeMachineService(repository);
            var orderRequest = new OrderRequest
            {
                Order = new Dictionary<string, int> { { "Americano", 1 } },
                Payment = new Payment
                {
                    TotalAmount = 1000,
                    Coins = new List<int> { 500, 500 },
                    Bills = new List<int>()
                }
            };

            // Act
            var result = service.ProcessPurchase(orderRequest);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Successful purchase.", result.Message);
        }

        [Fact]
        public void ProcessPurchase_ShouldReturnFailureResponse_WhenPaymentIsNull()
        {
            // Arrange
            var service = new CoffeeMachineService(new MockCoffeeMachineRepository());
            var orderRequest = new OrderRequest
            {
                Order = new Dictionary<string, int> { { "Americano", 1 } },
                Payment = null
            };

            // Act
            var result = service.ProcessPurchase(orderRequest);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Payment information is required.", result.Message);
        }

        [Fact]
        public void HasEnoughChange_ShouldReturnTrue_WhenChangeIsZero()
        {
            // Arrange
            var service = new CoffeeMachineService(new MockCoffeeMachineRepository());

            // Act
            var result = service.HasEnoughChange(0);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetAvailableCoffees_ShouldReturnDictionary_WhenRepositoryHasCoffees()
        {
            // Arrange
            var service = new CoffeeMachineService(new MockCoffeeMachineRepository());

            // Act
            var result = service.GetAvailableCoffees();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Dictionary<string, int>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetCoffeePrices_ShouldReturnDictionary_WhenRepositoryHasCoffees()
        {
            // Arrange
            var service = new CoffeeMachineService(new MockCoffeeMachineRepository());

            // Act
            var result = service.GetCoffeePrices();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(950, result["Americano"]);
            Assert.Equal(1200, result["Cappuccino"]);
        }
    }
}