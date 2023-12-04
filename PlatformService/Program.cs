using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Added Services
if(builder.Environment.IsProduction())
{
    Console.WriteLine($"Production Model --> SQL Data Base");
    builder.Services.AddDbContext<AppDbContext>( opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}
else 
{
   Console.WriteLine($"Developer Mode --> In Memory Data Base");
   builder.Services.AddDbContext<AppDbContext>( opt => opt.UseInMemoryDatabase("InMem"));
}

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
// AppContext.SetSwitch(
// "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
//
builder.Services.AddGrpc();
#endregion




Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandService"]}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// 
app.MapGrpcService<GrpcPlatformService>();

//Optional -> Just to Share the contract .proto with the client
// app.MapGet("/protos/platforms.proto", async context => {
//     await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
// });

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();

