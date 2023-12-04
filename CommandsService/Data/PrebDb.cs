using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data;

public static class PrebDb
{


    public static void PrepPopulation(IApplicationBuilder applicationBuilder)
    {
         using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
         {
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
            var platforms = grpcClient.ReturnAllPlatforms();
            SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(),  platforms);
         }
    }

    private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
    {
         
         Console.WriteLine("--> Seeding new Platforms");
          
        foreach (var platform in platforms)
        {
            if(!repo.ExternalPlatformExits(platform.ExternalID))
            {
                repo.CreatePlatform(platform);
                repo.SaveChanges();
            }
        }

         

    }
}