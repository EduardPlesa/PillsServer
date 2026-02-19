using MediatR;

namespace PillsServer.Common
{
    public class BaseRequest<T> : IRequest<T> where T : BaseResponse
    {
    }
}
