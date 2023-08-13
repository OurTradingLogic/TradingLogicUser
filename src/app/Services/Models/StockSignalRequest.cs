using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.Services.Models;
public class StockSignalList
{
    public List<Stock> StockList {get; set;} = new();
    public List<Signal> SignalList {get; set;} = new();
}

public class Signal
{
    public string Name {get; set;} = string.Empty;
    public string Code {get; set;} = string.Empty;
}