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
                            $"Moneda no permitida: {coin}. " +
                            "Solo se aceptan monedas de 500, 100, 50, 25 colones.");
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
                            $"Billete no permitido: {bill}. " +
                            "Solo se aceptan billetes de 1000 colones.");
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
                    $"El monto total ({TotalAmount}) no coincide con la suma de " +
                    $"monedas y billetes ({calculatedTotal}).");
            }
            
            return (true, string.Empty);
        }

        // Validates all payment aspects at once
        public (bool isValid, string errorMessage) ValidatePayment()
        {
            // Validate denominations
            var (isValidDenom, denomError) = ValidateDenominations();
            if (!isValidDenom)
            {
                return (false, denomError);
            }
            
            // Validate total matches denominations
            var (isValidTotal, totalError) = ValidateTotalMatchesDenominations();
            if (!isValidTotal)
            {
                return (false, totalError);
            }
            
            // Validate total amount is positive
            if (TotalAmount <= 0)
            {
                return (false, "El monto total debe ser mayor a 0.");
            }
            
            return (true, string.Empty);
        }
    }
}