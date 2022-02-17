using CreditLineAnalyser.Web.Contracts.Requests;
using CreditLineAnalyser.Web.Dtos;
using CreditLineAnalyser.Web.Interfaces;

namespace CreditLineAnalyser.Web.Services;

public class CreditLineProcessor : ICreditLineProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Dictionary<string, Type> _analysers;

    public CreditLineProcessor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        _analysers = new Dictionary<string, Type>
        {
            { "SME", typeof(SmeAnalyser) },
            { "Startup", typeof(StartupAnalyser) },
        };
    }

    public CreditLineResult ProcessRequest(CreditLineRequest request)
    {
        using var scope = _scopeFactory.CreateScope();
        var foundType = _analysers.TryGetValue(request.FoundingType, out var analyserTpe);
        if (!foundType)
        {
            return new CreditLineResult(new[] { $"Could not analyse credit line of type {request.FoundingType}" });
        }

        var analyser = (ICreditLineAnalyser)scope.ServiceProvider.GetRequiredService(analyserTpe);

        return analyser.Analyse(request);
    }
}
