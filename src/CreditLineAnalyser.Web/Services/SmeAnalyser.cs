using CreditLineAnalyser.Web.Contracts.Requests;
using CreditLineAnalyser.Web.Dtos;
using CreditLineAnalyser.Web.Interfaces;

namespace CreditLineAnalyser.Web.Services;

public class SmeAnalyser : ICreditLineAnalyser
{
    public CreditLineResult Analyse(CreditLineRequest request)
    {
        var recomendedCredit = request.MonthlyRevenue / 5M;

        if (recomendedCredit > request.RequestedCreditLine)
        {
            return new CreditLineResult
            {
                Success = true,
                SuccessMessage = "Credit Line Accepted"
            };
        }

        return new CreditLineResult(new[] { "Credit line requested is too high" });
    }
}
