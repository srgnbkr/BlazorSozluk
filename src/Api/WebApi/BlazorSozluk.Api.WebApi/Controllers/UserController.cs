﻿using BlazorSozluk.Common.ViewModels.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginUserCommand command)
        {
            var result =  await mediator.Send(command);
            return Ok(result);
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}
