using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId
{
    public class GetStudentTriageDataBySurveyIdQueryHandler : IRequestHandler<GetStudentTriageDataBySurveyIdQuery, GetStudentTriageDataBySurveyIdResult>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public GetStudentTriageDataBySurveyIdQueryHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetStudentTriageDataBySurveyIdResult> Handle(GetStudentTriageDataBySurveyIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetWithResponseCode<GetStudentTriageDataBySurveyIdResponse>(new GetStudentTriageDataBySurveyIdRequest(request.SurveyGuid));

            result.EnsureSuccessStatusCode();

            return new GetStudentTriageDataBySurveyIdResult
            {
                Id = result.Body.Id,
                LepsId = result.Body.LepsId,
                LogId = result.Body.LogId,
                FirstName = result.Body.FirstName,
                LastName = result.Body.LastName,
                DateOfBirth = result.Body.DateOfBirth,
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