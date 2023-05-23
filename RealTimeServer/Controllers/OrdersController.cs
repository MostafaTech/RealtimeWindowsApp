using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace RealTimeServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IHubContext<SignalR.RealtimeHub> _hub;
        public OrdersController(
            IHubContext<SignalR.RealtimeHub> hub)
        {
            _hub = hub;
        }

        [HttpPost]
        public async Task<IActionResult> OrderCreate([FromBody] OrderCreateRequest request)
        {
            await _hub.Clients.Groups(request.BranchId).SendAsync("print", request.FoodName);
            return Ok();
        }
    }

    public class OrderCreateRequest
    {
        public string BranchId { get; set; }
        public string FoodName { get; set; }
    }
}
