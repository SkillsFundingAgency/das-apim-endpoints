using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public sealed record GetUsersByEmployerAccountIdRequest(long EmployerAccountId): IGetApiRequest
{
    public string GetUrl => $"api/user/by/employerAccountId/{EmployerAccountId}";
}