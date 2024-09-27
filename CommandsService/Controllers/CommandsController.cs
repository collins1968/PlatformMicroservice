using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
namespace CommandService.Controller
{
    [Route("api/platformsss/{platformId}/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;
        public CommandController(ICommandRepo commandService, IMapper mapper)
        {
            _commandRepo = commandService;
            _mapper = mapper;
        }

        [HttpGet]   
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform", platformId);

            if(!_commandRepo.PlatformExists(platformId))
            {
                return NotFound();
            }
            var commands = _commandRepo.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetcommandForPlatform")]

        public ActionResult<CommandReadDto> GetcommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine("--> Hit Getcommandforplatform:{0} commandID {1}", platformId, commandId);
            if (!_commandRepo.PlatformExists(platformId))
            {
                return NotFound();
            }
            var command = _commandRepo.GetCommand(platformId, commandId);

            if(command == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandReadDto commandDto)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform", platformId);

            if (!_commandRepo.PlatformExists(platformId))
            {
                return NotFound();
            }
            var command = _mapper.Map<Command>(commandDto);

            _commandRepo.CreateCommand(command, platformId);
            _commandRepo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(_commandRepo);

            return CreatedAtRoute(nameof(GetcommandForPlatform),
                new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
        }
        }
}