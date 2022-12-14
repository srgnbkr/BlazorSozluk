using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.ViewModels.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Queries.GetEntries
{
    public class GetEntriesQueryHandler : IRequestHandler<GetEntriesQuery, List<GetEntriesViewModel>>
    {
        private readonly IEntryRepository entryRepository;
        private readonly IMapper mapper;

        public GetEntriesQueryHandler(IEntryRepository entryRepository, IMapper mapper)
        {
            this.entryRepository = entryRepository;
            this.mapper = mapper;
        }

        public async Task<List<GetEntriesViewModel>> Handle(GetEntriesQuery request, CancellationToken cancellationToken)
        {
            //Query ile daha hızlı sorgulamalar yapıyoruz.
            var query = entryRepository.AsQueryable();

            if (request.TodaysEntries)
            {
                query = query
                    .Where(i => i.CreateDate.Date == DateTime.Now.Date)
                    .Where(i => i.CreateDate.Date == DateTime.Now.AddDays(1).Date);

            }
            
            query.Include(i => i.EntryComments)
                .OrderByDescending(i => Guid.NewGuid())
                .Take(request.Count);

            //Automapper extensions method

            return await query.ProjectTo<GetEntriesViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
