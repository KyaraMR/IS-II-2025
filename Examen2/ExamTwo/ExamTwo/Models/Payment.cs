using System.Collections.Generic;

namespace ExamTwo.Models
{
    // Represents the payment made by the customer
    public class Payment
    {
        public int TotalAmount { get; set; }
        public List<int> Coins { get; set; } // List of coin denominations inserted (500, 100, 50, 25)
        public List<int> Bills { get; set; } // List of bill denominations inserted (only 1000 allowed)
        public Payment()
        {
            Coins = new List<int>();
            Bills = new List<int>();
        }

        // Validates that only allowed denominations are used
        public (bool isValid, string errorMessage) ValidateDenominations()
        {
            // Validate coins
            if (Coins != null)
            {
                foreach (var coin in Coins)
                {
                    if (coin != 500 && coin != 100 && coin != 50 && coin != 25)
                    {
                        return (false, 
                            $"Invalid coin: {coin}. " +
                            "The only coins allowed are 500, 100, 50, 25 colones.");
                    }
                }
            }
            
            // Validate bills (only 1000 colones allowed)
            if (Bills != null)
            {
                foreach (var bill in Bills)
                {
                    if (bill != 1000)
                    {
                        return (false, 
                            $"Invalid bill: {bill}. " +
                            "The only bills allowed are 1000 colones.");
                    }
                }
            }
            
            return (true, string.Empty);
        }

        // Calculates total from coins and bills
        public int CalculateTotalFromDenominations()
        {
            int total = 0;
            if (Coins != null) total += Coins.Sum();
            if (Bills != null) total += Bills.Sum();
            return total;
        }

        // Validates that TotalAmount matches sum of coins and bills
        public (bool isValid, string errorMessage) ValidateTotalMatchesDenominations()
        {
            int calculatedTotal = CalculateTotalFromDenominations();
            
            if (TotalAmount != calculatedTotal)
            {
                return (false, 
                    $"The total amount ({TotalAmount}) does not match the sum of " +
                    $"coins and bills ({calculatedTotal}).");
            }
            
            return (true, string.Empty);
        }
    }
}