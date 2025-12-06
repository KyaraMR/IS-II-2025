using System.Collections.Generic;
using ExamTwo.Models;

namespace ExamTwo.Services.Interfaces
{
    // Service interface containing business logic for coffee machine operations
    public interface ICoffeeMachineService
    {
        // Query methods
        Dictionary<string, int> GetAvailableCoffees();
        Dictionary<string, int> GetCoffeePrices();
        Dictionary<string, int> GetCoffeeQuantities();
        
        // Validation methods
        bool ValidateOrder(OrderRequest orderRequest, out string errorMessage);
        bool ValidatePayment(OrderRequest orderRequest, int totalCost, out string errorMessage);

        CoffeeMachineResponse ProcessPurchase(OrderRequest orderRequest);  
        bool HasEnoughChange(int changeAmount);
        string FormatPurchaseResponse(CoffeeMachineResponse result);
    }
}