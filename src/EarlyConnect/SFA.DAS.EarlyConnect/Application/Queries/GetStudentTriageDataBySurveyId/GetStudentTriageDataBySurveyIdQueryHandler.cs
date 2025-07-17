﻿using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId
{
    public class GetStudentTriageDataBySurveyIdQueryHandler : IRequestHandler<GetStudentTriageDataBySurveyIdQuery, GetStudentTriageDataResult>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public GetStudentTriageDataBySurveyIdQueryHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetStudentTriageDataResult> Handle(GetStudentTriageDataBySurveyIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetWithResponseCode<GetStudentTriageDataResponse>(new GetStudentTriageDataBySurveyIdRequest(request.SurveyGuid));

            result.EnsureSuccessStatusCode();

            return new GetStudentTriageDataResult
            {
                Id = result.Body.Id,
                LepDateSent = result.Body.LepDateSent,
                LepsId = result.Body.LepsId,
                LepCode = result.Body.LepCode,
                LogId = result.Body.LogId,
                FirstName = result.Body.FirstName,
                LastName = result.Body.LastName,
                DateOfBirth = result.Body.DateOfBirth,
                SchoolName = result.Body.SchoolName,
                URN=result.Body.URN,
                Email = result.Body.Email,
                Telephone = result.Body.Telephone,
                Postcode = result.Body.Postcode,
                DataSource = result.Body.DataSource,
                Industry = result.Body.Industry,
                DateInterest = result.Body.DateInterest,
                StudentSurvey = result.Body.StudentSurvey,
                SurveyQuestions = result.Body.SurveyQuestions
            };
        }
    }
}