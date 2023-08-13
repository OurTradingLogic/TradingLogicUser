using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

[Table("Users")]
public class Users
{
    [Key]
    public int Id {get; set;}

    [StringLength(50)]
    [Required]
    public string UserName {get;set;} = string.Empty; 

    [StringLength(50)]
    [Required]
    public string Name {get;set;} = string.Empty; 

    [StringLength(50)]
    [Required]
    public string PhoneNumber {get;set;} = string.Empty; 

    [StringLength(50)]
    [Required]
    public string EmailId {get;set;} = string.Empty; 

    [StringLength(50)]
    public string? MoreDetails {get;set;}
}