using Microsoft.AspNetCore.Mvc;
using ExamTwo.Models;
using ExamTwo.Services.Interfaces;

namespace ExamTwo.Controllers
{
    // API Controller for coffee machine operations
    // Handles HTTP requests and delegates business logic to service layer
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeMachineController : ControllerBase
    {
        private readonly ICoffeeMachineService _coffeeMachineService;
        
        // Constructor with dependency injection
        public CoffeeMachineController(ICoffeeMachineService coffeeMachineService)
        {
            _coffeeMachineService = coffeeMachineService;
        }
        
        // GET endpoint to retrieve available coffees and quantities
        [HttpGet("coffees")]
        public ActionResult<Dictionary<string, int>> GetAvailableCoffees()
        {
            var coffees = _coffeeMachineService.GetAvailableCoffees();
            return Ok(coffees);
        }
        
        // GET endpoint to retrieve coffee prices
        [HttpGet("prices")]
        public ActionResult<Dictionary<string, int>> GetCoffeePrices()
        {
            var prices = _coffeeMachineService.GetCoffeePrices();
            return Ok(prices);
        }
        
        // GET endpoint to retrieve coffee quantities
        [HttpGet("quantities")]
        public ActionResult<Dictionary<string, int>> GetCoffeeQuantities()
        {
            var quantities = _coffeeMachineService.GetCoffeeQuantities();
            return Ok(quantities);
        }
        
        // POST endpoint to process a coffee purchase
        [HttpPost("purchase")]
        public ActionResult PurchaseCoffee([FromBody] OrderRequest request)
        {
            if (request == null)
            {
                return BadRequest("Solicitud inválida.");
            }
            
            if (request.Payment == null)
            {
                return BadRequest("Se requiere información de pago.");
            }
            
            // Optional: Check if at least some payment method is provided
            bool hasCoins = request.Payment.Coins != null && request.Payment.Coins.Count > 0;
            bool hasBills = request.Payment.Bills != null && request.Payment.Bills.Count > 0;
            bool hasTotalAmount = request.Payment.TotalAmount > 0;
            
            if (!hasCoins && !hasBills && !hasTotalAmount)
            {
                return BadRequest("Debe proporcionar monedas, billetes o un monto total.");
            }
            
            // Process purchase through service
            var result = _coffeeMachineService.ProcessPurchase(request);
            
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            
            // Format response according to requirements
            string responseMessage = FormatPurchaseResponse(result);
            
            return Ok(responseMessage);
        }
        
        // POST endpoint to check coffee availability without purchasing
        [HttpPost("check-availability")]
        public ActionResult CheckCoffeeAvailability([FromBody] Dictionary<string, int> order)
        {
            // Create a temporary request to reuse validation logic
            var tempRequest = new OrderRequest
            {
                Order = order,
                Payment = new Payment { TotalAmount = 0 }
            };
            
            // Use service validation without actually processing purchase
            var isValid = _coffeeMachineService.ValidateOrder(tempRequest, out string errorMessage);
            
            if (!isValid)
            {
                return BadRequest(errorMessage);
            }
            
            return Ok("Disponibilidad confirmada.");
        }

        private string FormatPurchaseResponse(CoffeeMachineResponse result)
        {
            if (result.ChangeAmount == 0)
            {
                return "Compra exitosa. No hay vuelto.";
            }
            
            string responseMessage = $"Su vuelto es de {result.ChangeAmount} colones.\nDesglose:";
            
            if (result.ChangeBreakdown != null && result.ChangeBreakdown.Count > 0)
            {
                // Sort coins from highest to lowest
                foreach (var coin in result.ChangeBreakdown.OrderByDescending(c => c.Key))
                {
                    responseMessage += $"\n{coin.Value} moneda(s) de {coin.Key}";
                }
            }
            
            return responseMessage;
        }
    }
}