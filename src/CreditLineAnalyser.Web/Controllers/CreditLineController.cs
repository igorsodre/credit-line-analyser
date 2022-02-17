using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreditLineAnalyser.Web.Contracts.Requests;
using CreditLineAnalyser.Web.Contracts.Responses;
using CreditLineAnalyser.Web.Dtos;
using CreditLineAnalyser.Web.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreditLineAnalyser.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditLineController : ControllerBase
    {
        private readonly ICreditLineProcessor _processor;

        public CreditLineController(ICreditLineProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost("")]
        public ActionResult PlaceCreditLine(CreditLineRequest request)
        {
            var result = _processor.ProcessRequest(request);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(result.ErrorMessages));
            }

            return Ok(new SuccessResponse<string>(result.SuccessMessage));
        }
    }
}
