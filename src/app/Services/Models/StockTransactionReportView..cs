using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.Services.Models;

public class StockTransactionReportView
{
    public string Stock{get; set;} = string.Empty;
    public int Holding { get; set; }
    public double AvgPrice { get; set; }
    public double? CurrentPrice { get; set; }
    public double? Profit { get; set; }
}