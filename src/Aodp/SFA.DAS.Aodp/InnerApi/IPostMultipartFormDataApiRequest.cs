namespace SFA.DAS.Aodp.InnerApi;

public interface IPostMultipartFormDataApiRequest
{
    string PostUrl { get; }

    IEnumerable<KeyValuePair<string, string>> FormData { get; }
}
