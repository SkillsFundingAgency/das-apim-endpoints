using MediatR;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate
{
    [ExcludeFromCodeCoverage]
    public class GetStudentTriageDataByDateQueryHandler : IRequestHandler<GetStudentTriageDataByDateQuery, List<GetStudentTriageDataResponse>>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public GetStudentTriageDataByDateQueryHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<GetStudentTriageDataResponse>> Handle(GetStudentTriageDataByDateQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetWithResponseCode<List<GetStudentTriageDataResponse>>(new GetStudentTriageDataByDateRequest(request.ToDate, request.FromDate));

            result.EnsureSuccessStatusCode();

            var listResult = new List<GetStudentTriageDataResponse>();

            foreach(var student in result.Body)
            {             
                listResult.Add(new GetStudentTriageDataResponse
                {
                    Id = student.Id,
                    LepDateSent = student.LepDateSent,
                    LepsId = student.LepsId,
                    LepCode = student.LepCode,
                    LogId = student.LogId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    DateOfBirth = student.DateOfBirth,
                    SchoolName = student.SchoolName,
                    URN = student.URN,
                    Email = student.Email,
                    Telephone = student.Telephone,
                    Postcode = student.Postcode,
                    DataSource = student.DataSource,
                    Industry = student.Industry,
                    DateInterest = student.DateInterest,
                    StudentSurvey = student.StudentSurvey,
                    SurveyQuestions = student.SurveyQuestions,
                });
            }

            return listResult;       
        }
    }
}