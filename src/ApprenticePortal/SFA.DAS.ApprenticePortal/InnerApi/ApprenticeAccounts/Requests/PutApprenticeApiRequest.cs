using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;

public class PutApprenticeApiRequest(PutApprenticeApiRequestData requestData) : IPutApiRequest
{
    public string PutUrl => "/apprentices";
    public object Data { get; set; } = requestData;
}

public class PutApprenticeApiRequestData
{
    public string Email { get; set; }
    public string GovUkIdentifier { get; set; }
}