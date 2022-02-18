using CreditLineAnalyser.Web.Contracts.Requests;
using CreditLineAnalyser.Web.Contracts.Responses;
using CreditLineAnalyser.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CreditLineAnalyser.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditLineController : ControllerBase
    {
        private readonly ICreditLineProcessor _processor;
        private readonly IThrottleService _throttleService;

        public CreditLineController(ICreditLineProcessor processor, IThrottleService throttleService)
        {
            _processor = processor;
            _throttleService = throttleService;
        }

        [HttpPost("")]
        public async Task<ActionResult> PlaceCreditLine(CreditLineRequest request)
        {
            var result = _processor.ProcessRequest(request);
            await _throttleService.StoreResponse(result);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(result.ErrorMessages));
            }

            return Ok(new SuccessResponse<string>(result.SuccessMessage));
        }
    }
}
