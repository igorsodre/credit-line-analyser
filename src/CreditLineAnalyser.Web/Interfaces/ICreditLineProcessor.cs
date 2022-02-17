using CreditLineAnalyser.Web.Contracts.Requests;
using CreditLineAnalyser.Web.Dtos;

namespace CreditLineAnalyser.Web.Interfaces;

public interface ICreditLineProcessor
{
    CreditLineResult ProcessRequest(CreditLineRequest request);
}
