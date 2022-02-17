using CreditLineAnalyser.Web.Contracts.Requests;
using CreditLineAnalyser.Web.Dtos;
using CreditLineAnalyser.Web.Interfaces;

namespace CreditLineAnalyser.Web.Services;

public class StartupAnalyser : ICreditLineAnalyser
{
    public CreditLineResult Analyse(CreditLineRequest request)
    {
        if (GetRecomendedCreditLine(request) > request.RequestedCreditLine)
        {
            return new CreditLineResult { Success = true };
        }

        return new CreditLineResult(new[] { "Credit line requested is too high" });
    }

    private decimal GetRecomendedCreditLine(CreditLineRequest request)
    {
        return Math.Max(request.CashBalance / 3M, request.MonthlyRevenue / 5M);
    }
}
