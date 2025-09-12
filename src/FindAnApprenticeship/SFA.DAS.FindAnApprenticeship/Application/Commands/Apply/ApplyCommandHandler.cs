using MediatR;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Shared;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply;

public class ApplyCommandHandler(
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    ILogger<ApplyCommandHandler> logger)
    : IRequestHandler<ApplyCommand, ApplyCommandResponse>
{
    public async Task<ApplyCommandResponse> Handle(ApplyCommand request, CancellationToken cancellationToken)
    {
        var result =
            await findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(
                new GetVacancyRequest(request.VacancyReference));

        if (!string.IsNullOrEmpty(result.ApplicationUrl))
        {
            logger.LogWarning("Application attempted to be created for {VacancyReference} with application url {ApplicationUrl}", request.VacancyReference, result.ApplicationUrl);
            return null;
        }
        
        var additionalQuestions = new List<KeyValuePair<int, string>>();
        if (result.AdditionalQuestion1 != null) { additionalQuestions.Add(new KeyValuePair<int, string>(1, result.AdditionalQuestion1)); }
        if (result.AdditionalQuestion2 != null) { additionalQuestions.Add(new KeyValuePair<int, string>(2, result.AdditionalQuestion2)); }

        // Check if the address is null or empty and set it to null if so: For Recruit National is address will be null
        var addresses = result.OtherAddresses is { Count: > 0 }
            ? new List<Address> { result.Address }.Concat(result.OtherAddresses).ToList()
            : result.Address != null ? [result.Address] : null;

        var putApplicationApiRequestData = new PutApplicationApiRequest.PutApplicationApiRequestData
        {
            AdditionalQuestions = additionalQuestions,
            ApprenticeshipType = result.ApprenticeshipType,
            CandidateId = request.CandidateId,
            IsAdditionalQuestion1Complete = string.IsNullOrEmpty(result.AdditionalQuestion1) ? (short)4 : (short)0,
            IsAdditionalQuestion2Complete = string.IsNullOrEmpty(result.AdditionalQuestion2) ? (short)4 : (short)0,
            IsDisabilityConfidenceComplete = result.IsDisabilityConfident ? (short)0 : (short)4,
            IsEmploymentLocationComplete = result.EmployerLocationOption is AvailableWhere.MultipleLocations ? (short)0 : (short)4,
            EmploymentLocation = result.EmployerLocationOption is not null ?
                new LocationDto
                {
                    Id = Guid.NewGuid(),
                    EmployerLocationOption = result.EmployerLocationOption,
                    EmploymentLocationInformation = result.EmploymentLocationInformation,
                    Addresses = addresses?.OrderByCity().Select((a, index) => new AddressDto
                    {
                        Id = Guid.NewGuid(),
                        IsSelected = false,
                        FullAddress = JsonConvert.SerializeObject(a),
                        AddressOrder = (short)(index + 1)
                    }).ToList()
                }
                : null,
        };
        var vacancyReference = request.VacancyReference.TrimVacancyReference();
        var putRequest = new PutApplicationApiRequest(vacancyReference, putApplicationApiRequestData);

        var applicationResult =
            await candidateApiClient.PutWithResponseCode<PutApplicationApiResponse>(putRequest);

        applicationResult.EnsureSuccessStatusCode();

        if (applicationResult is null) return null;

        return new ApplyCommandResponse
        {
            ApplicationId = applicationResult.Body.Id
        };
    }
}