using Application.CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class DataIngetionController(IMediator mediator) : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var result = await mediator.Send(new UploadFileCommand(file));
            var response = ApiResponse.Success(result);
             return Ok(response);
    }

        [HttpGet("status/{id:guid}")]
        public async Task<IActionResult> GetStatus(Guid id)
        {
            var status = await mediator.Send(new GetUploadStatusQuery(id));
            var response = ApiResponse.Success(status);
            return Ok(response);
    }
    }

