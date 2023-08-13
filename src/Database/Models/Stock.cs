using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

[Table("StockList")]
public class Stock
{
    [Key]
    public int Id {get; set;}

    [StringLength(50)]
    [Required]
    public string Symbol {get;set;} = string.Empty; 

    [StringLength(50)]
    [Required]
    public string Name {get;set;}  = string.Empty;

    [StringLength(50)]
    public string? Description {get;set;}
}