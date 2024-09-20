using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsssController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;
        public PlatformsssController(ICommandRepo repo, IMapper mapper)
        {
            _commandRepo = repo;
            _mapper = mapper;
        }

        [HttpGet]

        public ActionResult<IEnumerable<PlatformReadDto>>  GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from CommandService");

            var platforms = _commandRepo.GetPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }
        [HttpPost]
        public ActionResult TestInBoundConnection()
        {            
            Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("Inbound test of from Platforms Controller");
        }
    }
}
