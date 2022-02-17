using CreditLineAnalyser.Web.Contracts.Requests;
using CreditLineAnalyser.Web.Dtos;

namespace CreditLineAnalyser.Web.Interfaces;

public interface ICreditLineAnalyser
{
    CreditLineResult Analyse(CreditLineRequest request);
};
