namespace SFA.DAS.Aodp.InnerApi;

public interface IPostMultipartJsonFileApiRequest
{
    string PostUrl { get; }
    object Data { get; }
}
