using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;
using ServiceLayer.Service;
using System.Threading.Tasks;


namespace KoiManagementSystem.Controllers.Order
{
    [ApiController]
    [Route("api/[controller]")]
    public class VnPayController : Controller
    {
        private readonly IVnPayService _orderService;

        public VnPayController(IVnPayService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("proceed-vnpay-payment")]
        public async Task<IActionResult> ProceedVnPayPayment([FromBody] string orderId)
        {
            try
            {
                
                var paymentUrl = await _orderService.CreatePaymentUrlAsync(int.Parse(orderId));
                return Ok(new { paymentUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
