using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace app.Services.Helpers
{
    public interface ICacheService
    {
        T GetTradingList<T>(string key);
        bool SetTradingList<T>(string key, T value);
        //bool Clear(string? inputKey = null);
    }

    public class CacheService: ICacheService
    {
        ObjectCache _memoryCache = MemoryCache.Default;

        public T GetTradingList<T>(string key)
        {
            T item = (T)_memoryCache.Get(key);
            return item;
        }

        public bool SetTradingList<T>(string key, T value)
        {
            bool response = true;

            if (!string.IsNullOrEmpty(key))
            {
                _memoryCache.Set(key, value, 
                new CacheItemPolicy { 
                    //SlidingExpiration = TimeSpan.FromMinutes(1),
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(2),
                    Priority = CacheItemPriority.Default
                    });
            }

            return response;  
        }
        
    }
}