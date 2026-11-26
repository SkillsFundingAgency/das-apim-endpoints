using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class PutAccountUserRequest(string userRef, string email, string firstName, string lastName, Guid? correlationId)
    : IPutApiRequest
{
    public string PutUrl => $"api/user/upsert";
    public object Data { get; set; } = new
    {
        UserRef = userRef,
        FirstName = firstName,
        LastName = lastName,
        EmailAddress = email,
        CorrelationId = correlationId.HasValue ? correlationId.Value.ToString() : string.Empty
    };
}