using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate
{
    public class GetStudentTriageDataByDateQueryHandler : IRequestHandler<GetStudentTriageDataByDateQuery, List<GetStudentTriageDataByDateResult>>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public GetStudentTriageDataByDateQueryHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<GetStudentTriageDataByDateResult>> Handle(GetStudentTriageDataByDateQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetWithResponseCode<List<GetStudentTriageDataByDateResponse>>(new GetStudentTriageDataByDateRequest(request.ToDate, request.FromDate));

            result.EnsureSuccessStatusCode();

            var listResult = new List<GetStudentTriageDataByDateResult>();

            foreach(var student in result.Body)
            {
                listResult.Add(new GetStudentTriageDataByDateResult
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