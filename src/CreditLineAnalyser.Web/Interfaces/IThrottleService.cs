using CreditLineAnalyser.Web.Dtos;

namespace CreditLineAnalyser.Web.Interfaces;

public interface IThrottleService
{
    Task Set(string key, string value);

    Task<CacheItem> Get(string key);
}
