namespace CreditLineAnalyser.Web.Contracts.Requests;

public class CreditLineRequest
{
    public string FoundingType { get; set; }

    public decimal CashBalance { get; set; }

    public decimal MonthlyRevenue { get; set; }

    public decimal RequestedCreditLine { get; set; }

    public DateTime RequestedDate { get; set; }
}
