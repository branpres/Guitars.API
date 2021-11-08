using Application.Authentication;
using Application.Features.Guitars.Queries.ReadGuitars;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guitars.API.OldWay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuitarsController : ControllerBase
    {
        private readonly ISender _mediator;

        public GuitarsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Policy = Constants.Policies.READ)]
        [HttpGet]
        public async Task<IActionResult> ReadGuitarsAsync(string? filter = null, int pageIndex = -1, int pageSize = -1)
        {
            var guitarsVM = await _mediator.Send(new ReadGuitarsQuery(filter, pageIndex == -1 ? null : pageIndex, pageSize == -1 ? null : pageSize));
            return Ok(guitarsVM);
        }
    }
}