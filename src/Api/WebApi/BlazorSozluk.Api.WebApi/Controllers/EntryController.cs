using BlazorSozluk.Api.Application.Features.Queries.GetEntries;
using BlazorSozluk.Api.Application.Features.Queries.GetEntryComments;
using BlazorSozluk.Api.Application.Features.Queries.GetMainPageEntries;
using BlazorSozluk.Common.ViewModels.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : BaseController
    {
        private readonly IMediator mediator;

        public EntryController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetEntries([FromQuery] GetEntriesQuery query)
        {
            var entries = await mediator.Send(query);

            return Ok(entries);
        }

        [HttpGet]
        [Route("MainPageEntries")]
        public async Task<IActionResult> GetMainPageEntries(int page, int pageSize)
        {
            var entries = await mediator.Send(new GetMainPageEntriesQuery(UserId, page, pageSize));

            return Ok(entries);
        }

        [HttpGet]
        [Route("Comments/{id}")]
        public async Task<IActionResult> GetEntryComments(Guid id, int page, int pageSize)
        {
            var result = await mediator.Send(new GetEntryCommentsQuery(id, UserId, page, pageSize));

            return Ok(result);
        }

        [HttpPost]
        [Route("CreateEntry")]
        [Authorize]
        public async Task<IActionResult> CreateEntry([FromBody] CreateEntryCommand command)
        {
            if (!command.CreatedById.HasValue)
                command.CreatedById = UserId;

            var result = await mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        [Route("CreateEntryComment")]
        [Authorize]
        public async Task<IActionResult> CreateEntryComment([FromBody] CreateEntryCommentCommand command)
        {
            if (!command.CreatedById.HasValue)
                command.CreatedById = UserId;

            var result = await mediator.Send(command);

            return Ok(result);
        }


        




    }
}
