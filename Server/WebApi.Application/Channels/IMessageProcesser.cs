namespace WebApi.Application.Channels
{
    public interface IMessageProcesser
    {
        void Process(string msg);
    }
}
