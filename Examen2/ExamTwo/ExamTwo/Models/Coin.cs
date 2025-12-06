namespace ExamTwo.Models
{
    public class Coin
    {
        // Represents a coin denomination available in the machine for change
        public int Denomination { get; set; } // 500, 100, 50, 25
        public int AvailableQuantity { get; set; }
        
        public Coin(int denomination, int availableQuantity)
        {
            Denomination = denomination;
            AvailableQuantity = availableQuantity;
        }
    }
}