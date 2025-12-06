namespace ExamTwo.Models
{
    // Standardized response format for coffee machine operations
    public class CoffeeMachineResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ChangeAmount { get; set; } // Total change to return
        public Dictionary<int, int> ChangeBreakdown { get; set; } // Change by denomination
    }
}