namespace ExamTwo.Models
{
    // Represents a coffee type available in the vending machine
    public class Coffee
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int AvailableQuantity { get; set; }
        
        public Coffee(string name, int price, int availableQuantity)
        {
            Name = name;
            Price = price;
            AvailableQuantity = availableQuantity;
        }
    }
}