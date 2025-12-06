using Xunit;
using ExamTwo.Repositories;

namespace ExamTwo.Tests.Repositories
{
    public class CoffeeMachineRepositoryTests
    {
        [Fact]
        public void Constructor_ShouldInitializeData_WhenRepositoryIsCreated()
        {
            // Arrange & Act
            var repository = new CoffeeMachineRepository();

            // Assert
            var coffees = repository.GetAllCoffees();
            var coins = repository.GetAllCoins();
            
            Assert.NotNull(coffees);
            Assert.NotEmpty(coffees);
            Assert.NotNull(coins);
            Assert.NotEmpty(coins);
        }

        [Fact]
        public void GetCoffeeByName_ShouldReturnCoffee_WhenCoffeeExists()
        {
            // Arrange
            var repository = new CoffeeMachineRepository();

            // Act
            var coffee = repository.GetCoffeeByName("Americano");

            // Assert
            Assert.NotNull(coffee);
            Assert.Equal("Americano", coffee.Name);
        }

        [Fact]
        public void UpdateCoffeeQuantity_ShouldUpdateQuantity_WhenCoffeeExists()
        {
            // Arrange
            var repository = new CoffeeMachineRepository();

            // Act
            repository.UpdateCoffeeQuantity("Americano", 5);
            var coffee = repository.GetCoffeeByName("Americano");

            // Assert
            Assert.Equal(5, coffee.AvailableQuantity);
        }

        [Fact]
        public void UpdateCoffeeQuantity_ShouldDoNothing_WhenCoffeeDoesNotExist()
        {
            // Arrange
            var repository = new CoffeeMachineRepository();

            // Act
            var exception = Record.Exception(() => 
                repository.UpdateCoffeeQuantity("Espresso", 5));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void GetCoinByDenomination_ShouldReturnCoin_WhenCoinExists()
        {
            // Arrange
            var repository = new CoffeeMachineRepository();

            // Act
            var coin = repository.GetCoinByDenomination(500);

            // Assert
            Assert.NotNull(coin);
            Assert.Equal(500, coin.Denomination);
            Assert.Equal(20, coin.AvailableQuantity);
        }

        [Fact]
        public void GetCoinByDenomination_ShouldReturnNull_WhenCoinDoesNotExist()
        {
            // Arrange
            var repository = new CoffeeMachineRepository();

            // Act
            var coin = repository.GetCoinByDenomination(200);

            // Assert
            Assert.Null(coin);
        }

        [Fact]
        public void UpdateCoinQuantity_ShouldUpdateQuantity_WhenCoinExists()
        {
            // Arrange
            var repository = new CoffeeMachineRepository();

            // Act
            repository.UpdateCoinQuantity(500, 15);
            var coin = repository.GetCoinByDenomination(500);

            // Assert
            Assert.Equal(15, coin.AvailableQuantity);
        }

        [Fact]
        public void GetAllCoffees_ShouldReturnList_WhenRepositoryIsInitialized()
        {
            // Arrange
            var repository = new CoffeeMachineRepository();

            // Act
            var coffees = repository.GetAllCoffees();

            // Assert
            Assert.NotNull(coffees);
            Assert.Equal(4, coffees.Count);
        }

        [Fact]
        public void GetAllCoins_ShouldReturnList_WhenRepositoryIsInitialized()
        {
            // Arrange
            var repository = new CoffeeMachineRepository();

            // Act
            var coins = repository.GetAllCoins();

            // Assert
            Assert.NotNull(coins);
            Assert.Equal(4, coins.Count);
        }

        [Fact]
        public void InitializeMachine_ShouldResetData_WhenCalled()
        {
            // Arrange
            var repository = new CoffeeMachineRepository();
            repository.UpdateCoffeeQuantity("Americano", 0);
            repository.UpdateCoinQuantity(500, 0);

            // Act
            repository.InitializeMachine();
            var coffee = repository.GetCoffeeByName("Americano");
            var coin = repository.GetCoinByDenomination(500);

            // Assert
            Assert.Equal(10, coffee.AvailableQuantity);
            Assert.Equal(20, coin.AvailableQuantity);
        }
    }
}
