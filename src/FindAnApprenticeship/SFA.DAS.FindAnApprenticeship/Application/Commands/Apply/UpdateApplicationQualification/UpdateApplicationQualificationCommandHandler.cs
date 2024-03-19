using System.Collections.Generic;
using System.Linq;
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
        var requests = request.Subjects.Select(subject => new PutApplicationQualificationApiRequest(request.CandidateId, request.ApplicationId, new PutApplicationQualificationApiRequestData
            {
                Grade = subject.Grade,
                Id = subject.Id,
                Subject = subject.Name,
                AdditionalInformation = subject.AdditionalInformation,
                IsPredicted = subject.IsPredicted,
                ToYear = subject.ToYear,
                QualificationReferenceId = request.QualificationReferenceId
            }))
            .Select(candidateApiClient.PutWithResponseCode<PutApplicationQualificationApiResponse>)
            .Cast<Task>()
            .ToList();


        await Task.WhenAll(requests);
        
        return new Unit();
    }
}