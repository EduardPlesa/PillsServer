using MediatR;
using Microsoft.EntityFrameworkCore;
using PillsServer.Common;
using PillsServer.Persistancy;

namespace PillsServer.CQRS.Commands
{
    public class PillCommandHandler(IRepo<Pill> repo) :
        IRequestHandler<AddPillCommand, AddPillCommandResponse>,
        IRequestHandler<UpdatePillCommand, UpdatePillCommandResponse>,
        IRequestHandler<DeletePillCommand, DeletePillCommandResponse>,
        IRequestHandler<TakePillCommand, TakePillCommandResponse>,
        IRequestHandler<TakeAllPillsCommand, TakeAllPillsCommandResponse>
    {
        public async Task<AddPillCommandResponse> Handle(AddPillCommand request, CancellationToken cancellationToken)
        {
            var pill = new Pill
            {
                Name = request.Name,
                Stock = request.Stock != null? request.Stock.Value : 0,
                PillsToTakePerTimeOfDay = request.PillsToTakePerTimeOfDay != null ? request.PillsToTakePerTimeOfDay : [0, 0, 0]
            };
            repo.Add(pill);
            await repo.SaveChangesAsync();
            return new AddPillCommandResponse { Id = pill.Id };
        }

        public async Task<UpdatePillCommandResponse> Handle(UpdatePillCommand request, CancellationToken cancellationToken)
        {
            var pill = await repo.Query(x => x.Id == request.Id).FirstOrDefaultAsync();
            if (pill == null) 
                return new UpdatePillCommandResponse { Errors = { "Error! This pill does not exist" } };
            if(request.Name != null)
            {
                pill.Name = request.Name;
            }
            if(request.Stock != null)
            {
                pill.Stock = request.Stock.Value;
            }
            if(request.PillsToTakePerTimeOfDay != null)
            {
                pill.PillsToTakePerTimeOfDay = request.PillsToTakePerTimeOfDay;
            }
            await repo.SaveChangesAsync();
            return new UpdatePillCommandResponse();
        }

        public async Task<DeletePillCommandResponse> Handle(DeletePillCommand request, CancellationToken cancellationToken)
        {
            var pill = await repo.Query(x => x.Id == request.Id).FirstOrDefaultAsync();
            if(pill == null)
                return new DeletePillCommandResponse { Errors = { "Error! This pill does not exist" } };
            repo.Remove(pill);
            await repo.SaveChangesAsync();
            return new DeletePillCommandResponse();
        }

        public async Task<TakePillCommandResponse> Handle(TakePillCommand request, CancellationToken cancellationToken)
        {
            var pill = await repo.Query(x => x.Id == request.Id).FirstOrDefaultAsync();
            if(pill == null)
                return new TakePillCommandResponse { Errors = { "Error! This pill does not exist" } };
            if(pill.Stock == 0)
                return new TakePillCommandResponse {};

            pill.Stock-=pill.PillsToTakePerTimeOfDay[GetPeriodOfDay()];
            pill.Stock = int.Max(pill.Stock, 0);
            pill.LastTaken = DateTime.Now;
            await repo.SaveChangesAsync();
            return new TakePillCommandResponse();
        }
        private int GetPeriodOfDay()
        {
            var now = DateTime.Now;
            if(now.Hour >= 6 && now.Hour < 12)
                return 0;
            if(now.Hour >= 12 && now.Hour < 18)
                return 1;
            return 2;
        }

        public async Task<TakeAllPillsCommandResponse> Handle(TakeAllPillsCommand request, CancellationToken cancellationToken)
        {
            var pills = repo.Query();
            await pills.ForEachAsync(pill =>
            {
                if(pill.Stock == 0)
                    return;
                pill.Stock -= pill.PillsToTakePerTimeOfDay[GetPeriodOfDay()];
                pill.Stock = int.Max(pill.Stock, 0);
                pill.LastTaken = DateTime.Now;
            });
            await repo.SaveChangesAsync();
            return new TakeAllPillsCommandResponse();
        }
    }
}
