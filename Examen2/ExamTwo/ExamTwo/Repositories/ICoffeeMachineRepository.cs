using System.Collections.Generic;
using ExamTwo.Models;

namespace ExamTwo.Repositories.Interfaces
{
    // Repository interface for coffee machine data access
    public interface ICoffeeMachineRepository
    {
        // Coffee operations
        List<Coffee> GetAllCoffees();
        Coffee GetCoffeeByName(string coffeeName);
        void UpdateCoffeeQuantity(string coffeeName, int newQuantity);
        
        // Coin operations
        List<Coin> GetAllCoins();
        Coin GetCoinByDenomination(int denomination);
        void UpdateCoinQuantity(int denomination, int newQuantity);
        
        void InitializeMachine();
    }
}