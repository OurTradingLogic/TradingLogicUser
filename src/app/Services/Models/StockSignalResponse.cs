using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.Services.Models;

public class SignalDetail
{
    public string Date { get; set; } = string.Empty;
    public string OnDate { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Signal { get; set; } = string.Empty;
    public string Tool { get; set; } = string.Empty;
}

public class IndicatorSignalCustomView
{
    private double _currentPrice;
    public string Stock {get; set;} = string.Empty;
    public string Tool { get; set; } = string.Empty;
    public string Signal { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string OnDate { get; set; } = string.Empty;
    public double Price { get; set; }
    public double? CurrentPrice { get { return Math.Round(_currentPrice, 2); } set { _currentPrice = value??0; } }
    public int Quantity { get; set; }
}

public class IndicatorSignalResponse
{
    public List<Dictionary<string, IEnumerable<SignalDetail>>>? StockSignalList {get; set;}
}
