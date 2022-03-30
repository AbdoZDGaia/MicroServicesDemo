using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/commands/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo commandRepo,
            IMapper mapper)
        {
            _commandRepo = commandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

            if (!_commandRepo.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commandItems = _commandRepo.GetCommandsByPlatformId(platformId);
            var result = _mapper.Map<IEnumerable<CommandReadDto>>(commandItems);
            return Ok(result);
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatformById")]
        public ActionResult<CommandReadDto> GetCommandForPlatformById(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatformById: {platformId} / {commandId}");

            if (!_commandRepo.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commandItem = _commandRepo.GetCommand(platformId, commandId);
            if (commandItem == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<CommandReadDto>(commandItem);
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

            if (!_commandRepo.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _commandRepo.CreateCommand(platformId, commandModel);
            _commandRepo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);
            return CreatedAtRoute(nameof(GetCommandForPlatformById), new { platformId, commandId = commandReadDto.Id }, commandReadDto);
        }
    }
}
