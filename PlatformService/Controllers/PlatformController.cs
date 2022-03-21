using AutoMapper;
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
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformController(IPlatformRepo repo,
            IMapper mapper,
            ICommandDataClient commandDataClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public IActionResult GetPlatforms()
        {
            Console.WriteLine("--> Getting platforms...");
            var platformItems = _repo.GetAllPlatforms();
            var results = _mapper.Map<IEnumerable<PlatformReadDto>>(platformItems);
            return Ok(results);
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public IActionResult GetPlatformById(int id)
        {
            Console.WriteLine("--> Getting platform by Id...");
            var platformItem = _repo.GetPlatformById(id);
            if (platformItem == null)
                return NotFound();
            var results = _mapper.Map<PlatformReadDto>(platformItem);
            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            Console.WriteLine("--> Creating platform");
            var platform = _mapper.Map<Platform>(platformCreateDto);
            _repo.CreatePlatform(platform);
            _repo.SaveChanges();

            var result = _mapper.Map<PlatformReadDto>(platform);

            try
            {
                await _commandDataClient.SendPlatformToCommand(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("--> Could not send platform synchronously: " + ex.Message); ;
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { id = result.Id }, result);
        }
    }
}
