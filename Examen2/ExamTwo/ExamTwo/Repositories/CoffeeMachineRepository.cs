using System.Collections.Generic;
using System.Linq;
using ExamTwo.Models;
using ExamTwo.Repositories.Interfaces;

namespace ExamTwo.Repositories
{
    // Concrete implementation of coffee machine repository
    public class CoffeeMachineRepository : ICoffeeMachineRepository
    {
        private List<Coffee> _coffees;
        private List<Coin> _coins;
        

        public CoffeeMachineRepository()
        {
            _coffees = new List<Coffee>();
            _coins = new List<Coin>();
            InitializeMachine();
        }
        
        public List<Coffee> GetAllCoffees()
        {
            return _coffees;
        }
        
        public Coffee GetCoffeeByName(string coffeeName)
        {
            // Search for coffee by name
            return _coffees.FirstOrDefault(c => c.Name == coffeeName);
        }
        
        public void UpdateCoffeeQuantity(string coffeeName, int newQuantity)
        {
            // Find coffee and update its quantity
            var coffee = GetCoffeeByName(coffeeName);
            if (coffee != null)
            {
                // Prevent negative quantities
                coffee.AvailableQuantity = newQuantity;
            }
        }
        
        public List<Coin> GetAllCoins()
        {
            return _coins;
        }
        
        public Coin GetCoinByDenomination(int denomination)
        {
            return _coins.FirstOrDefault(c => c.Denomination == denomination);
        }
        
        public void UpdateCoinQuantity(int denomination, int newQuantity)
        {
            // Find coin and update its quantity
            var coin = GetCoinByDenomination(denomination);
            if (coin != null)
            {
                // Prevent negative quantities
                coin.AvailableQuantity = newQuantity;
            }
        }
        
        // Initializes the machine with default inventory
        public void InitializeMachine()
        {
            // Init coffees
            _coffees = new List<Coffee>
            {
                new Coffee("Americano", 950, 10),
                new Coffee("Cappuccino", 1200, 8),
                new Coffee("Lates", 1350, 10),
                new Coffee("Mocaccino", 1500, 15)
            };
            
            // Init coins
            _coins = new List<Coin>
            {
                new Coin(500, 20),
                new Coin(100, 30),
                new Coin(50, 50),
                new Coin(25, 25)
            };
        }
    }
}