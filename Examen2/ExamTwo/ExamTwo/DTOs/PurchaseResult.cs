using System.Collections.Generic;

namespace ExamTwo.DTOs
{
    // Data Transfer Object for purchase results
    public class PurchaseResult
    {
        public string Message { get; set; }
        public int TotalChange { get; set; }
        public Dictionary<int, int> ChangeBreakdown { get; set; }
    }
}