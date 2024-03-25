using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.CoursesApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CoursesApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;

public class GetAddQualificationQueryHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ICacheStorageService cacheStorageService) : IRequestHandler<GetAddQualificationQuery,GetAddQualificationQueryResult>
{
    public async Task<GetAddQualificationQueryResult> Handle(GetAddQualificationQuery request, CancellationToken cancellationToken)
    {
        var qualificationTypeTask = apiClient.GetWithResponseCode<GetQualificationReferenceTypesApiResponse>(
            new GetQualificationReferenceTypesApiRequest());
        var qualificationsTask = apiClient.GetWithResponseCode<GetQualificationsApiResponse>(
            new GetQualificationsApiRequest(request.ApplicationId, request.CandidateId,
                request.QualificationReferenceTypeId));

        await Task.WhenAll(qualificationTypeTask, qualificationsTask);
        
        var result = qualificationTypeTask.Result.Body.QualificationReferences
            .FirstOrDefault(x => x.Id == request.QualificationReferenceTypeId);

        var courseList = await GetCoursesForApprenticeships(result);
        
        return new GetAddQualificationQueryResult
        {
            Courses = courseList,
            QualificationType = result,
            Qualifications = request.Id == null 
                ? qualificationsTask.Result.Body.Qualifications 
                : qualificationsTask.Result.Body.Qualifications.Where(c=>c.Id == request.Id).ToList()
        };
    }

    private async Task<List<GetAddQualificationQueryResult.CourseResponse>> GetCoursesForApprenticeships(QualificationReference result)
    {
        var courseList = new List<GetAddQualificationQueryResult.CourseResponse>();
        if (result != null && result.Name.Equals("Apprenticeship", StringComparison.CurrentCultureIgnoreCase))
        {
            var standards = await cacheStorageService.RetrieveFromCache<GetStandardsApiResponse>(nameof(GetStandardsApiResponse));
            var frameworks = await cacheStorageService.RetrieveFromCache<GetFrameworksApiResponse>(nameof(GetFrameworksApiResponse));

            if (standards == null)
            {
                standards = await coursesApiClient.Get<GetStandardsApiResponse>(new GetAllStandardsRequest());
                await cacheStorageService.SaveToCache(nameof(GetStandardsApiResponse), standards,
                    TimeSpan.FromHours(12));
            }
            courseList.AddRange(standards.Standards.Select(standard => 
                new GetAddQualificationQueryResult.CourseResponse
                {
                    Id = standard.StandardUId, 
                    Title = standard.Title, 
                    IsStandard = true
                }));

            if (frameworks == null)
            {
                frameworks = await coursesApiClient.Get<GetFrameworksApiResponse>(new GetAllFrameworksApiRequest());
                await cacheStorageService.SaveToCache(nameof(GetFrameworksApiResponse), frameworks,
                    TimeSpan.FromHours(12));
            }
            courseList.AddRange(frameworks.Frameworks.Select(framework => 
                new GetAddQualificationQueryResult.CourseResponse
                {
                    Id = framework.Id, 
                    Title = framework.Title, 
                    IsStandard = false
                }));
        }

        return courseList;
    }
}