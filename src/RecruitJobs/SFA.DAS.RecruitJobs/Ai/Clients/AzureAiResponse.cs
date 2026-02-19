using System.ClientModel;
using System.Linq;
using System.Net;
using System.Text.Json;
using OpenAI.Chat;

namespace SFA.DAS.RecruitJobs.Ai.Clients;

public class AzureAiResponse<T> where T: class
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Refusal { get; set; }
    public T? Result { get; set; }
    public string RawResult { get; set; }

    public static AzureAiResponse<T> From(ClientResult<ChatCompletion> clientResult)
    {
        var rawResponse = clientResult.GetRawResponse();
        var chatCompletion = clientResult.Value;
        if (chatCompletion is null)
        {
            return new AzureAiResponse<T>
            {
                StatusCode = (HttpStatusCode)rawResponse.Status,
            };
        }
        
        var chatMessageContentPart = chatCompletion.Content.FirstOrDefault();
        var result = new AzureAiResponse<T>
        {
            Refusal = chatCompletion.Refusal,
            StatusCode = (HttpStatusCode)rawResponse.Status,
        };

        if (chatMessageContentPart is not null)
        {
            result.RawResult = chatMessageContentPart.Text;
            result.Result = JsonSerializer.Deserialize<T>(chatMessageContentPart.Text);
        }
        
        return result;
    }
    
    public static AzureAiResponse<T> From(ClientResultException exception)
    {
        return new AzureAiResponse<T>
        {
            Refusal = exception.Message,
            StatusCode = (HttpStatusCode)exception.Status,
        };
    }
}