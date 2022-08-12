using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.ViewModels.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.EntryComment.Create
{
    public class CreateEntryCommentCommandHandler : IRequestHandler<CreateEntryCommentCommand, Guid>
    {
        private readonly IEntryCommentRepository entryCommentRepository;

        public CreateEntryCommentCommandHandler(IEntryCommentRepository entryCommentRepository,IMapper mapper)
        {
            this.entryCommentRepository = entryCommentRepository;
            this.mapper = mapper;
        }

        private readonly IMapper mapper;
        public async Task<Guid> Handle(CreateEntryCommentCommand request, CancellationToken cancellationToken)
        {
            var entryComment = mapper.Map<Domain.Models.EntryComment>(request);
            await entryCommentRepository.AddAsync(entryComment);
            return entryComment.Id;
        }
    }
}
