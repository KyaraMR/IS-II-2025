using System.Collections.Generic;

namespace ExamTwo.Models
{
    // Represents the payment made by the customer
    public class Payment
    {
        public int TotalAmount { get; set; }
        public List<int> Coins { get; set; } // List of coin denominations inserted (500, 100, 50, 25)
        public List<int> Bills { get; set; } // List of bill denominations inserted (only 1000 allowed)
    }
}