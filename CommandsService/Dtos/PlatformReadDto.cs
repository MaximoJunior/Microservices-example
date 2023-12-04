using System.ComponentModel.DataAnnotations;
using CommandsService.Models;

namespace CommandsService.Dtos;

public class PlatformReadDto
{
    public int Id { get; set; } 
    public string Name { get; set; }
}