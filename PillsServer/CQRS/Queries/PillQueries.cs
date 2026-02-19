using PillsServer.Common;

namespace PillsServer.CQRS.Queries
{
    public class GetPillQuery : BaseRequest<GetPillQueryResponse>
    {
        public required int Id { get; set; }
    }
    public class GetPillsQuery : BaseRequest<GetPillsQueryResponse>
    {
    }

    public class GetPillsQueryResponse : BaseResponse
    {
        public List<Pill> Pills { get; set; }
    }

    public class GetPillQueryResponse : BaseResponse
    {
        public Pill Pill { get; set; }
    }
}
