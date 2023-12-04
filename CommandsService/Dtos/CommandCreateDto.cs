using System.ComponentModel.DataAnnotations;
using CommandsService.Models;

namespace CommandsService.Dtos;

public class CommandCreateDto
{
    [Required]
    public string HowTo { get; set;}
     [Required]
    public string CommandLine { get; set; }
}