using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(
       IPlatformRepo repository,
       IMapper mapper,
       ICommandDataClient commandDataClient,
       IMessageBusClient messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        var platformItems = _repository.GetAllPlatforms();
        var list = _mapper.Map<IEnumerable<PlatformReadDto>>(platformItems);
        return Ok(list);
    }


    [HttpGet("{id}", Name = nameof(GetPlatformById))]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatformById([FromRoute] int id)
    {
        var platform = _repository.GetPlatformById(id);
        var item = _mapper.Map<PlatformReadDto>(platform);

        if (item == null) return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform([FromBody] PlatformCreateDto createDto)
    {
        if (ModelState.IsValid)
        {
            var platform = _mapper.Map<Platform>(createDto);
            _repository.CreatePlatform(platform);
            _repository.SaveChanges();
            var platformCreated = _mapper.Map<PlatformReadDto>(platform);

            // Send Sync Message
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformCreated);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            //Send Async Message
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformCreated);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }


            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformCreated.Id }, platformCreated);
        }

        return BadRequest(ModelState);
    }


}