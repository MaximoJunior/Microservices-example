using System.ComponentModel.DataAnnotations;
using CommandsService.Models;

namespace CommandsService.Dtos;

public class CommandReadDto
{
    public int Id { get; set; }
    public string HowTo { get; set;}
    public string CommandLine { get; set; }
    public int PlatformID { get; set; }
}