using System;
using System.Collections.Generic;
using System.Linq;
using ExamTwo.Models;
using ExamTwo.Repositories.Interfaces;
using ExamTwo.Services.Interfaces;

namespace ExamTwo.Services
{
    // Main service implementing coffee machine business logic
    public class CoffeeMachineService : ICoffeeMachineService
    {
        private readonly ICoffeeMachineRepository _repository;
        
        public CoffeeMachineService(ICoffeeMachineRepository repository)
        {
            _repository = repository;
        }
        
        public Dictionary<string, int> GetAvailableCoffees()
        {
            var coffees = _repository.GetAllCoffees();
            return coffees.ToDictionary(c => c.Name, c => c.AvailableQuantity);
        }
        
        public Dictionary<string, int> GetCoffeePrices()
        {
            var coffees = _repository.GetAllCoffees();
            return coffees.ToDictionary(c => c.Name, c => c.Price);
        }
        
        public Dictionary<string, int> GetCoffeeQuantities()
        {
            return GetAvailableCoffees();
        }
        
        public bool ValidateOrder(OrderRequest orderRequest, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Check if order is empty
            if (orderRequest == null || orderRequest.Order == null || orderRequest.Order.Count == 0)
            {
                errorMessage = "Orden vacía.";
                return false;
            }
            
            // Check if payment is valid
            if (orderRequest.Payment == null || orderRequest.Payment.TotalAmount <= 0)
            {
                errorMessage = "Monto de pago inválido.";
                return false;
            }
            
            // Validate each item in the order
            foreach (var orderItem in orderRequest.Order)
            {
                var coffee = _repository.GetCoffeeByName(orderItem.Key);
                if (coffee == null)
                {
                    errorMessage = $"El café '{orderItem.Key}' no existe.";
                    return false;
                }
                
                // Check if enough quantity is available
                if (coffee.AvailableQuantity < orderItem.Value)
                {
                    errorMessage = $"No hay suficientes unidades de {orderItem.Key}. Disponibles: {coffee.AvailableQuantity}";
                    return false;
                }
            }
            
            return true;
        }
        
        public bool ValidatePayment(OrderRequest orderRequest, int totalCost, out string errorMessage)
        {
            errorMessage = string.Empty;
            
            // Check if payment amount is sufficient
            if (orderRequest.Payment.TotalAmount < totalCost)
            {
                errorMessage = "Dinero insuficiente para realizar la compra.";
                return false;
            }
            
            return true;
        }
        
        public bool HasEnoughChange(int changeAmount)
        {
            // No change needed
            if (changeAmount <= 0) return true;
            
            var coins = _repository.GetAllCoins();
            // Sort coins from highest to lowest denomination
            var sortedCoins = coins.OrderByDescending(c => c.Denomination).ToList();
            
            int remainingChange = changeAmount;
            
            // Try to make change using available coins
            foreach (var coin in sortedCoins)
            {
                if (coin.AvailableQuantity > 0)
                {
                    // Calculate maximum coins of this denomination can be used
                    // Limited by needed amount and available quantity
                    int coinsNeeded = Math.Min(remainingChange / coin.Denomination, coin.AvailableQuantity);
                    remainingChange -= coinsNeeded * coin.Denomination;
                }
                
                if (remainingChange == 0) break;
            }

            // Success only if the exact change was made
            return remainingChange == 0;
        }
        
        public CoffeeMachineResponse ProcessPurchase(OrderRequest orderRequest)
        {
            var response = new CoffeeMachineResponse();
            
            // Validate order
            if (!ValidateOrder(orderRequest, out string errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }
            
            // Calculate total cost
            int totalCost = 0;
            foreach (var orderItem in orderRequest.Order)
            {
                var coffee = _repository.GetCoffeeByName(orderItem.Key);
                totalCost += coffee.Price * orderItem.Value;
            }
            
            // Validate payment
            if (!ValidatePayment(orderRequest, totalCost, out errorMessage))
            {
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }
            
            // Calculate change amount
            int changeAmount = orderRequest.Payment.TotalAmount - totalCost;
            
            // Verify sufficient change
            if (!HasEnoughChange(changeAmount))
            {
                response.Success = false;
                response.Message = "No hay suficiente cambio en la máquina. La máquina está fuera de servicio.";
                return response;
            }
            
            // Update coffee quantities
            foreach (var orderItem in orderRequest.Order)
            {
                var coffee = _repository.GetCoffeeByName(orderItem.Key);
                int newQuantity = coffee.AvailableQuantity - orderItem.Value;
                _repository.UpdateCoffeeQuantity(orderItem.Key, newQuantity);
            }
            
            // Calculate and update change
            var changeBreakdown = CalculateChange(changeAmount);
            if (changeBreakdown == null)
            {
                response.Success = false;
                response.Message = "Fallo al realizar la compra - no se puede dar cambio exacto.";
                return response;
            }
            
            // Update coin quantities
            foreach (var coinChange in changeBreakdown)
            {
                var coin = _repository.GetCoinByDenomination(coinChange.Key);
                int newQuantity = coin.AvailableQuantity - coinChange.Value;
                _repository.UpdateCoinQuantity(coin.Denomination, newQuantity);
            }
            
            response.Success = true;
            response.Message = "Compra realizada exitosamente.";
            response.ChangeAmount = changeAmount;
            response.ChangeBreakdown = changeBreakdown;
            
            return response;
        }
        
        // Calculates the optimal coin breakdown for a given change amount
        private Dictionary<int, int> CalculateChange(int changeAmount)
        {
            // No change needed
            if (changeAmount <= 0) return new Dictionary<int, int>();
            
            var coins = _repository.GetAllCoins();
            // Sort coins from highest to lowest denomination
            var sortedCoins = coins.OrderByDescending(c => c.Denomination).ToList();
            var changeBreakdown = new Dictionary<int, int>();
            
            int remainingChange = changeAmount;
            
            // Use largest coin first
            foreach (var coin in sortedCoins)
            {
                // Only use coins that are available
                if (coin.AvailableQuantity > 0)
                {
                    int coinsNeeded = Math.Min(remainingChange / coin.Denomination, coin.AvailableQuantity);
                    if (coinsNeeded > 0)
                    {
                        changeBreakdown[coin.Denomination] = coinsNeeded;
                        // Reduce remaining change amount
                        remainingChange -= coinsNeeded * coin.Denomination;
                    }
                }
                
                if (remainingChange == 0) break;
            }
            
            // Return null if couldn't make exact change
            return remainingChange == 0 ? changeBreakdown : null;
        }
    }
}