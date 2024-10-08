using Microsoft.AspNetCore.Mvc;
using Server.Web.Lobby;

namespace Server.Web.Controllers;

[ApiController]
[Route("api/waiters")]
public class WaitersController : Controller
{
    private readonly IWaiterList _waitersList;
    
    public WaitersController(IWaiterList waitersList)
    {
        _waitersList = waitersList;
    }

    [HttpPost("connect/{id}")]
    public async Task<IActionResult> ConnectWaiter(string id)
    {
        await _waitersList.Add(id);
        
        return Ok();
    }
    
    [HttpPost("disconnect/{id}")]
    public async Task<IActionResult> DisconnectWaiter(string id)
    {
        await _waitersList.Remove(id);
            
        return Ok();
    }
    
    [HttpGet("waiters")]
    public async Task<IActionResult> GetAll()
    {
        var all = await _waitersList.GetAll();
        
        return Ok(all);
    }
    
    [HttpGet("waiters/{id}")]
    public async Task<IActionResult> GetAllExcept(string id)
    {
        var all = await _waitersList.GetAllExcept(id);
        
        return Ok(all);
    }
}