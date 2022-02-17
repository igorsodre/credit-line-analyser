namespace CreditLineAnalyser.Web.Contracts.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse() { }

        public ErrorResponse(IEnumerable<string> errorMessages)
        {
            Errors = errorMessages;
        }

        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }
}
