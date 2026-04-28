using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class PutUpsertEmployerUserAccountRequest(
    Guid userId,
    string govIdentifier,
    string email,
    string firstName,
    string lastName)
    : IPutApiRequest
{
    public string PutUrl => $"api/users/{userId}";
    public object Data { get; set; } = new
    {
        GovIdentifier = govIdentifier,
        FirstName = firstName,
        LastName = lastName,
        Email = email
    };
}