using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.Services.Models;

public class StockTransactionReportView
{
    private double _avgPrice;
    private double _currentPrice;
    private double _profit;
    public string Stock{get; set;} = string.Empty;
    public int Holding { get; set; }
    public double AvgPrice { get { return Math.Round(_avgPrice, 2); } set { _avgPrice = value; } }
    public double? CurrentPrice { get { return Math.Round(_currentPrice, 2); } set { _currentPrice = value??0; } }
    public double? Profit { get { return Math.Round(_profit, 2); } set { _profit = value??0; } }
}