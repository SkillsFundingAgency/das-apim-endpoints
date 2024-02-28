using System.Xml.Serialization;
using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;

public class GetNhsJobsQueryHandler(INhsJobsApiClient<NhsJobsConfiguration> nhsJobsApiClient, ILiveVacancyMapper liveVacancyMapper) : IRequestHandler<GetNhsJobsQuery, GetNhsJobsQueryResult>
{
    public async Task<GetNhsJobsQueryResult> Handle(GetNhsJobsQuery request, CancellationToken cancellationToken)
    {
        var result = await GetNhsPageResult(1);

        if (result?.Vacancies == null || result.Vacancies.Count == 0)
        {
            return new GetNhsJobsQueryResult
            {
                NhsVacancies = []
            };
        }
        
        var vacancies = result.Vacancies.Select(liveVacancyMapper.Map).ToList();
        
        for (var i = 2; result.TotalPages >= i; i++)
        {
            result = await GetNhsPageResult(i);

            vacancies.AddRange(result.Vacancies.Select(liveVacancyMapper.Map).ToList());
        }
        

        return new GetNhsJobsQueryResult
        {
            NhsVacancies = vacancies.ToList()
        };
    }

    private async Task<GetNhsJobApiResponse?> GetNhsPageResult(int pageNumber)
    {
        var apiResponse = await nhsJobsApiClient.GetWithResponseCode<string>(new GetNhsJobsApiRequest(pageNumber));

        if ((int)apiResponse.StatusCode >= 300)
        {
            return null;
        }
        
        var xmlSerializer = new XmlSerializer(typeof(GetNhsJobApiResponse));
        using TextReader reader = new StringReader(apiResponse.Body);
        var result = xmlSerializer.Deserialize(reader);

        return (GetNhsJobApiResponse)result!;
    }
}