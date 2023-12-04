using CommandsService.Models;

namespace CommandsService.Data;

public interface ICommandRepo
{
    bool SaveChanges();

    //Platforms
    IEnumerable<Platform> GetAllPlatforms();
    void CreatePlatform(Platform plat);
    bool PlatformExits(int platformId);

    bool ExternalPlatformExits(int externalPlatformId);

    //Commands
    IEnumerable<Command> GetCommandsForPlatform(int platformId);
    Command GetCommand(int platformId, int commandId);
    void CreateCommand(int platformId, Command command);
}