using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateApplicationQualification;

public class UpdateApplicationQualificationCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<UpdateApplicationQualificationCommand, Unit>
{
    public async Task<Unit> Handle(UpdateApplicationQualificationCommand request, CancellationToken cancellationToken)
    {
        var apiRequest = new PutApplicationQualificationApiRequest(request.CandidateId, request.ApplicationId,
            new PutApplicationQualificationApiRequestData
            {
                Grade = request.Grade,
                Id = request.Id,
                Subject = request.Subject,
                AdditionalInformation = request.AdditionalInformation,
                IsPredicted = request.IsPredicted,
                ToYear = request.ToYear,
                QualificationReferenceId = request.QualificationReferenceId
            });

        await candidateApiClient.PutWithResponseCode<PutApplicationQualificationApiResponse>(apiRequest);
        
        return new Unit();
    }
}