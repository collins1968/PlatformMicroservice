using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandData;
        public PlatformController(IPlatformRepo repository, IMapper mapper, ICommandDataClient commandData)
        {
            _repository = repository;
            _mapper = mapper;
            _commandData = commandData;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlartforms()
        {
            var plartforms = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(plartforms));
        }

        [HttpGet("Id")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
             var platform = _repository.GetPlatformById(id);    
            if (GetAllPlartforms == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatForm(PlatformCreateDto platformCreateDto)
        {
            var createdPlatform = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(createdPlatform);
           _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(createdPlatform);

            try
            {
                await _commandData.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }
            return Ok(platformReadDto);
        }
    }
}
