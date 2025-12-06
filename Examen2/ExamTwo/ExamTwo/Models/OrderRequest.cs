using System.Collections.Generic;

namespace ExamTwo.Models
{
    // Represents an order request from the customer
    public class OrderRequest
    {
        // Dictionary with coffee name as key and requested quantity as value
        public Dictionary<string, int> Order { get; set; }
        // Payment details (total amount and breakdown of coins)
        public Payment Payment { get; set; }
    }
}