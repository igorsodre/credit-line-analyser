using CreditLineAnalyser.Web.Contracts.Requests;
using CreditLineAnalyser.Web.Dtos;
using CreditLineAnalyser.Web.Interfaces;

namespace CreditLineAnalyser.Web.Services;

public class StartupAnalyser : ICreditLineAnalyser
{
    public CreditLineResult Analyse(CreditLineRequest request)
    {
        throw new NotImplementedException();
    }
}
