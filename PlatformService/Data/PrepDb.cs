using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction){

          using(var serviceScop = app.ApplicationServices.CreateScope())
          {
             var appDbContext = serviceScop.ServiceProvider.GetService<AppDbContext>();
         
             SeedData(appDbContext, isProduction);
          }
    }

    private static void SeedData(AppDbContext context, bool isProduction)
    {

         if(isProduction)
         {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                  context.Database.Migrate();
            }
            catch(Exception ex)
            {
                 Console.WriteLine($"--> Migration Fail: {ex.Message}");
            }
            
         }

         if(!context.Platforms.Any())
         {
            Console.WriteLine("---> Seeding Data...");

            context.Platforms.AddRange(
                new Platform(){ Name = "Test", Publisher = "Junior", Cost = "Test"},
                new Platform(){ Name = "Test", Publisher = "Junior", Cost = "Test"}
            );

            context.SaveChanges();
         }
         else 
         {
            Console.WriteLine("---> We alreday have data");
         }
    }
}