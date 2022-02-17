namespace CreditLineAnalyser.Web.Contracts.Requests;

public class CreditLineRequest
{
    public string FoundingType { get; set; }

    public double CashBalance { get; set; }

    public double MonthlyRevenue { get; set; }

    public int RequestedCreditLine { get; set; }

    public DateTime RequestedDate { get; set; }
}
