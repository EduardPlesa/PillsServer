using MediatR;
using Microsoft.EntityFrameworkCore;
using PillsServer.Common;
using PillsServer.Persistancy;

namespace PillsServer.CQRS.Queries
{
    public class PillQueriesHandler :
        IRequestHandler<GetPillQuery, GetPillQueryResponse>,
        IRequestHandler<GetPillsQuery, GetPillsQueryResponse>
    {
        private readonly IRepo<Pill> pillRepository;
        public PillQueriesHandler(IRepo<Pill> pillRepository)
        {
            this.pillRepository = pillRepository;
        }

        public async Task<GetPillQueryResponse> Handle(GetPillQuery request, CancellationToken cancellationToken)
        {
            var pill = await pillRepository.Query(x => x.Id == request.Id).FirstOrDefaultAsync();
            if(pill == null)
                return new GetPillQueryResponse { Errors = { "Error! This pill does not exist" } };
            return new GetPillQueryResponse { Pill = pill };
        }

        public async Task<GetPillsQueryResponse> Handle(GetPillsQuery request, CancellationToken cancellationToken)
        {
            var pills = await pillRepository.Query().ToListAsync();
            return new GetPillsQueryResponse { Pills = pills };
        }
    }
}
