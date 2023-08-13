using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.Services.Models;
public class StockSignal
{
    public Stock Stock {get; set;} = new();
    public List<SignalAPI> SignalAPI {get; set;} = new();
    public int Holding {get; set;}
}

public class StockSignalResponse
{
    public List<StockSignal> StockSignalList {get; set;} = new();
}