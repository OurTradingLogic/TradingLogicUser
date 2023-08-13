using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

[Table("TransactionHistory")]
public class TransactionHistory
{
    [Key]
    public int Id {get; set;}

    public int UserId {get;set;} 

    public int StockId {get;set;} 

    [StringLength(50)]
    [Required]
    public string Type {get;set;} = string.Empty; 

    public int Quantity {get;set;}

    [Required]
    public DateTime DateAndTime {get;set;} 

    public double Amount {get;set;} 

    [StringLength(50)]
    public string? ExtraDetail {get;set;}
}