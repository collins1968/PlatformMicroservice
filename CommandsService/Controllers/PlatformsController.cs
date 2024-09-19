using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsssController : ControllerBase
    {
        public PlatformsssController()
        {
            
        }
        [HttpPost]
        public ActionResult TestInBoundConnection()
        {            
            Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("Inbound test of from Platforms Controller");
        }
    }
}
