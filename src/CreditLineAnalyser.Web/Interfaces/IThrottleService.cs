using CreditLineAnalyser.Web.Dtos;

namespace CreditLineAnalyser.Web.Interfaces;

public interface IThrottleService
{
    Task StoreResponse(CreditLineResult result);

    Task<ThrottleResult> GetResponse();
}
