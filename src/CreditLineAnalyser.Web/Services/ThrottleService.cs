using System.Text.Json;
using CreditLineAnalyser.Web.Constants;
using CreditLineAnalyser.Web.Contracts.Responses;
using CreditLineAnalyser.Web.Dtos;
using CreditLineAnalyser.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace CreditLineAnalyser.Web.Services;

public class ThrottleService : IThrottleService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public ThrottleService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task StoreResponse(CreditLineResult result)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var cacheItem = new ThrottleCacheItem<CreditLineResult>(result);
        await db.StringSetAsync(ThrottlingSettings.RequestKey, JsonSerializer.Serialize(cacheItem));
    }

    public async Task<ThrottleResult> GetResponse()
    {
        var db = _connectionMultiplexer.GetDatabase();
        var stringCacheItem = await db.StringGetAsync(ThrottlingSettings.RequestKey);
        if (stringCacheItem.IsNullOrEmpty)
        {
            return new ThrottleResult { ShouldThrottle = false };
        }

        var cacheItem = JsonSerializer.Deserialize<ThrottleCacheItem<CreditLineResult>>(stringCacheItem);

        if (cacheItem.Data.Success)
        {
            return await ThrotleSuccessCase(cacheItem, db);
        }

        return await ThrotleErrorCase(cacheItem, db);
    }

    private async Task<ThrottleResult> ThrotleErrorCase(
        ThrottleCacheItem<CreditLineResult> cacheItem,
        IDatabaseAsync database
    )
    {
        if (CacheOlderThenThirtySeconds(cacheItem))
        {
            await InvalidateCachedItem(database);
            return new ThrottleResult { ShouldThrottle = false };
        }

        cacheItem.CreatedAt = DateTime.Now;
        await IncrementCachedResultCounter(cacheItem, database);
        if (cacheItem.AccessCounter > 3)
        {
            return new ThrottleResult
            {
                ShouldThrottle = true,
                Response = new BadRequestObjectResult(new ErrorResponse(new[] { "A sales agent will contact you" }))
            };
        }

        return new ThrottleResult
        {
            ShouldThrottle = true,
            Response = new StatusCodeResult(StatusCodes.Status429TooManyRequests)
        };
    }

    private async Task<ThrottleResult> ThrotleSuccessCase(
        ThrottleCacheItem<CreditLineResult> cacheItem,
        IDatabaseAsync database
    )
    {
        if (CacheOlderThenTwoMinutes(cacheItem))
        {
            await InvalidateCachedItem(database);
            return new ThrottleResult { ShouldThrottle = false };
        }

        if (cacheItem.AccessCounter < 2)
        {
            await IncrementCachedResultCounter(cacheItem, database);
            return new ThrottleResult
            {
                ShouldThrottle = true,
                Response = new OkObjectResult(new SuccessResponse<string>(cacheItem.Data.SuccessMessage))
            };
        }

        return new ThrottleResult
        {
            ShouldThrottle = true,
            Response = new StatusCodeResult(StatusCodes.Status429TooManyRequests)
        };
    }

    private static async Task IncrementCachedResultCounter(
        ThrottleCacheItem<CreditLineResult> cacheItem,
        IDatabaseAsync database
    )
    {
        cacheItem.AccessCounter++;
        await database.StringSetAsync(ThrottlingSettings.RequestKey, JsonSerializer.Serialize(cacheItem));
    }

    private static bool CacheOlderThenTwoMinutes(ThrottleCacheItem<CreditLineResult> cacheItem)
    {
        return (DateTime.Now - cacheItem.CreatedAt).TotalSeconds > 2 * 60;
    }

    private bool CacheOlderThenThirtySeconds(ThrottleCacheItem<CreditLineResult> cacheItem)
    {
        return (DateTime.Now - cacheItem.CreatedAt).TotalSeconds > 5;
    }

    private async Task InvalidateCachedItem(IDatabaseAsync database)
    {
        await database.KeyDeleteAsync(ThrottlingSettings.RequestKey);
    }

    public class ThrottleCacheItem<T> where T : class
    {
        public ThrottleCacheItem() { }

        public ThrottleCacheItem(T data)
        {
            Data = data;
            CreatedAt = DateTime.Now;
            AccessCounter = 0;
        }

        public DateTime CreatedAt { get; set; }

        public int AccessCounter { get; set; }

        public T Data { get; set; }
    }
}
