using System.Collections.Generic;
using ExamTwo.Models;

namespace ExamTwo.Services.Interfaces
{
    // Service interface containing business logic for coffee machine operations
    public interface ICoffeeMachineService
    {
        // Queries
        Dictionary<string, int> GetAvailableCoffees();
        Dictionary<string, int> GetCoffeePrices();
        Dictionary<string, int> GetCoffeeQuantities();
        
        bool ValidateOrder(OrderRequest orderRequest, out string errorMessage);
        bool ValidatePayment(OrderRequest orderRequest, int totalCost, out string errorMessage);
        CoffeeMachineResponse ProcessPurchase(OrderRequest orderRequest);  
        bool HasEnoughChange(int changeAmount);
    }
}