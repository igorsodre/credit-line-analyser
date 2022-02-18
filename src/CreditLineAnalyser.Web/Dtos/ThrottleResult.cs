using Microsoft.AspNetCore.Mvc;

namespace CreditLineAnalyser.Web.Dtos;

public class ThrottleResult
{
    public bool ShouldThrottle { get; set; }

    public IActionResult Response { get; set; }
}
