using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.InnerApi;

public class PutApprenticeApiRequest(PutApprenticeApiRequestData requestData) : IPutApiRequest
{
    public string PutUrl => "/apprentices";
    public object Data { get; set; } = requestData;
}

public class PutApprenticeApiRequestData
{
    public required string Email { get; set; }
    public required string GovUkIdentifier { get; set; }
}