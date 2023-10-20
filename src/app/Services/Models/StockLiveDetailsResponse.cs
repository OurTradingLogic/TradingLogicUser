using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.Services.Models;

public class StockLiveDetail
{
    public double Price { get; set; }
}

public class StockLiveDetailsResponse
{
    public Dictionary<string, StockLiveDetail> StockLiveDetails {get; set;} = new(StringComparer.InvariantCultureIgnoreCase);
}