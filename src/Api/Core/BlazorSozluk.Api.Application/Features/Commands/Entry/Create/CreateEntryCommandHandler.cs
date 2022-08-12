using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.ViewModels.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.Entry.Create
{
    public class CreateEntryCommandHandler : IRequestHandler<CreateEntryCommand, Guid>
    {
        private readonly IEntryRepository entryRepository;
        private readonly IMapper mapper;


        public CreateEntryCommandHandler(IEntryRepository entryRepository,IMapper mapper)
        {
            this.mapper = mapper;
            this.entryRepository = entryRepository;
        }

        public async Task<Guid> Handle(CreateEntryCommand request, CancellationToken cancellationToken)
        {
            var entry = mapper.Map<Domain.Models.Entry>(request);
            await entryRepository.AddAsync(entry);

            return entry.Id;
        }
    }
}
