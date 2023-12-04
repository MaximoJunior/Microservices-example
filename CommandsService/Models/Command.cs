using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommandsService.Models;

public class Command
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string HowTo { get; set;}
    [Required]
    public string CommandLine { get; set; }
    [Required]
    public int PlatformID { get; set; }
    // [ForeignKey("PlatformID")]
    public Platform Platform { get; set; }
}