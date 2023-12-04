using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("/api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    
    private readonly ICommandRepo _repository;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform([FromRoute] int platformId)
    {
        Console.WriteLine("--> Hit GetCommandsForPlatform");
        
        if(!_repository.PlatformExits(platformId))
        {
            return NotFound();
        }

        var commands = _repository.GetCommandsForPlatform(platformId);
        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform([FromRoute] int platformId, [FromRoute] int commandId)
    {
        Console.WriteLine("--> Hit GetCommandForPlatform");
        
        if(!_repository.PlatformExits(platformId))
        {
            return NotFound();
        }

        var command = _repository.GetCommand(platformId, commandId);

        if(command == null) return NotFound();

        return Ok(_mapper.Map<CommandReadDto>(command));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommand([FromRoute] int platformId, [FromBody] CommandCreateDto commandDto)
    {
        Console.WriteLine("--> Hit CreateCommand");
        
        if(!_repository.PlatformExits(platformId))
        {
            return NotFound();
        }

        var commandToCreate = _mapper.Map<Command>(commandDto);

        _repository.CreateCommand(platformId, commandToCreate);
        _repository.SaveChanges();
        
         var commandReadDto = _mapper.Map<CommandReadDto>(commandToCreate);

        return CreatedAtRoute(
                  nameof(GetCommandForPlatform), 
                  new { 
                        platformId = platformId,  
                        commandId = commandReadDto.Id
                      }, 
                  commandReadDto);
    }
    

}