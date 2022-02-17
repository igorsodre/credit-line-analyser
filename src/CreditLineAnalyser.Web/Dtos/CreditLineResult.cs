namespace CreditLineAnalyser.Web.Dtos;

public class CreditLineResult
{
    public bool Success { get; set; } = false;

    public CreditLineResult() { }

    public CreditLineResult(string[] errorMessages)
    {
        ErrorMessages = errorMessages;
    }

    public string[] ErrorMessages { get; set; } = Array.Empty<string>();

    public string SuccessMessage { get; set; }
}
