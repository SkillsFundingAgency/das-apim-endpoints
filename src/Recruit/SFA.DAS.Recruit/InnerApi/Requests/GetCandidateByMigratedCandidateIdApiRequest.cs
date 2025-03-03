using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public class GetCandidateByMigratedCandidateIdApiRequest(Guid migratedCandidateId) : IGetApiRequest
{
    public string GetUrl => $"api/candidates/migrated/{migratedCandidateId}";
}