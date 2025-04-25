using MediatR;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SFA.DAS.EarlyConnect.Application.Queries;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate
{
    public class GetStudentDataBeforeDateQueryHandler : IRequestHandler<GetStudentDataBeforeDateQuery, List<GetStudentTriageDataBySurveyIdResult>>
    {
      
        private readonly ILogger<GetStudentDataBeforeDateQueryHandler> _logger;
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public GetStudentDataBeforeDateQueryHandler(ILogger<GetStudentDataBeforeDateQueryHandler> logger, IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public async Task<List<GetStudentTriageDataBySurveyIdResult>> Handle(GetStudentDataBeforeDateQuery request, CancellationToken cancellationToken)
        { 
            try
            {
                var result = await _apiClient.GetWithResponseCode<Collection<GetStudentTriageDataBySurveyIdResponse>>(new GetStudentTriageDataByDateRequest());
                result.EnsureSuccessStatusCode();
                var response = new List<GetStudentTriageDataBySurveyIdResult>();
                foreach (var item in result.Body)
                {
                    var dataitem = new GetStudentTriageDataBySurveyIdResult
                    {
                        Id = item.Id,
                        LepDateSent = item.LepDateSent,
                        LepsId = item.LepsId,
                        LepCode = item.LepCode,
                        LogId = item.LogId,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        DateOfBirth = item.DateOfBirth,
                        SchoolName = item.SchoolName,
                        URN = item.URN,
                        Email = item.Email,
                        Telephone = item.Telephone,
                        Postcode = item.Postcode,
                        DataSource = item.DataSource,
                        Industry = item.Industry,
                        DateInterest = item.DateInterest,
                        StudentSurvey = item.StudentSurvey,
                        SurveyQuestions = item.SurveyQuestions
                    };
                    response.Add(dataitem);
                   
                }
                return response;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling inner API: {ex.Message}");
                return null; // Or throw an exception
            }
        }
    }
}