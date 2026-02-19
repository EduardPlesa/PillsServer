using MediatR;
using PillsServer.Common;

namespace PillsServer.CQRS.Commands
{
    public class AddPillCommand : BaseRequest<AddPillCommandResponse>
    {
        public required string Name { get; set; }
        public int? Stock { get; set; }
        public int[]? PillsToTakePerTimeOfDay { get; set; }
    }
    public class UpdatePillCommand : BaseRequest<UpdatePillCommandResponse>
    {
        public required int Id { get; set; }
        public string? Name { get; set; }
        public int? Stock { get; set; }
        public int[]? PillsToTakePerTimeOfDay { get; set; }
    }
    public class DeletePillCommand : BaseRequest<DeletePillCommandResponse>
    {
        public required int Id { get; set; }
    }
    public class TakePillCommand : BaseRequest<TakePillCommandResponse>
    {
        public required int Id { get; set; }
    }
    public class TakeAllPillsCommand : BaseRequest<TakeAllPillsCommandResponse>
    {
    }

    public class TakeAllPillsCommandResponse : BaseResponse
    {
    }

    public class TakePillCommandResponse : BaseResponse
    {
    }

    public class DeletePillCommandResponse : BaseResponse
    {
    }

    public class UpdatePillCommandResponse : BaseResponse
    {
    }

    public class AddPillCommandResponse : BaseResponse
    {
        public int Id { get; set; }
    }
}
