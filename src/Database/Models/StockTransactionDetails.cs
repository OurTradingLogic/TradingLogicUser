using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

[Table("StockTransactionDetails")]
public class StockTransactionDetails
{
    [Key]
    public int Id {get; set;}

    public int UserId {get;set;} 

    public int StockId {get;set;} 

    [StringLength(100)]
    [Required]
    public string StockName {get;set;} = string.Empty;

    [StringLength(20)]
    [Required]
    public string Type {get;set;} = string.Empty; 

    public int Quantity {get;set;}

    [Required]
    public DateTime DateAndTime {get;set;} 

    /* Total Price, based on Quantity */
    public double Price {get;set;} 

    /* This field is null, since sell type only have profit value */
    public double? Profit {get;set;} 

    [StringLength(50)]
    public string? ExtraDetail {get;set;}
}