namespace PillsServer.Common
{
    public class BaseResponse
    {
        public bool Ok { get; set; } = true;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
