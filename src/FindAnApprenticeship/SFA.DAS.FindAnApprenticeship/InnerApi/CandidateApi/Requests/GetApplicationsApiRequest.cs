using System;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class GetApplicationsApiRequest : IGetApiRequest
{
    private readonly Guid _candidateId;
    private readonly ApplicationStatus? _status;

    public GetApplicationsApiRequest(Guid candidateId, ApplicationStatus? status = null)
    {
        _candidateId = candidateId;
        _status = status;
    }

    public string GetUrl => _status.HasValue
            ? $"api/candidates/{_candidateId}/applications?status={_status}"
            : $"api/candidates/{_candidateId}/applications";
}