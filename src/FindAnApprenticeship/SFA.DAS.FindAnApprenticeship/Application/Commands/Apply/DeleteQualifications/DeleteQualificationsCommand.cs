﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteQualifications
{
    public class DeleteQualificationsCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid QualificationReferenceId { get; set; }
        public Guid? Id { get; set; }
    }

    public class DeleteQualificationsCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<DeleteQualificationsCommand>
    {
        public async Task Handle(DeleteQualificationsCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {
                var apiRequest = new DeleteQualificationApiRequest(request.CandidateId, request.ApplicationId, request.Id.Value);
                await candidateApiClient.Delete(apiRequest);
            }
            else
            {
                var apiRequest = new DeleteQualificationsByTypeApiRequest(request.ApplicationId, request.CandidateId, request.QualificationReferenceId);
                await candidateApiClient.Delete(apiRequest);
            }
        }
    }
}
