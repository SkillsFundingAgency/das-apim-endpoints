using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.CoursesApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CoursesApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;

public class WhenHandlingGetAddQualificationQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_To_CandidateApi_Is_Made_And_QualificationReference_Returned_With_Qualifications(
        GetQualificationReferenceTypesApiResponse apiResponse,
        GetQualificationsApiResponse apiResponseQualifications,
        GetAddQualificationQuery query,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetAddQualificationQueryHandler handler
    )
    {
        query.Id = null;
        query.QualificationReferenceTypeId = apiResponse.QualificationReferences.Last().Id;
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetQualificationReferenceTypesApiResponse>(
                It.IsAny<GetQualificationReferenceTypesApiRequest>())).ReturnsAsync(
                new ApiResponse<GetQualificationReferenceTypesApiResponse>(apiResponse, HttpStatusCode.OK, ""));
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetQualificationsApiResponse>(
                It.Is<GetQualificationsApiRequest>(
                    c=>c.GetUrl.Contains(query.ApplicationId.ToString())
                    && c.GetUrl.Contains(query.CandidateId.ToString())
                    && c.GetUrl.Contains(query.QualificationReferenceTypeId.ToString())
                    ))).ReturnsAsync(
                new ApiResponse<GetQualificationsApiResponse>(apiResponseQualifications, HttpStatusCode.OK, ""));
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.QualificationType.Should()
            .BeEquivalentTo(apiResponse.QualificationReferences.FirstOrDefault(c => c.Id == query.QualificationReferenceTypeId));
        actual.Qualifications.Should().BeEquivalentTo(apiResponseQualifications.Qualifications);
    }
    [Test, MoqAutoData]
    public async Task Then_The_Request_To_CandidateApi_Is_Made_And_Single_Qualification_Returned_If_There_Is_An_Id(
        GetQualificationReferenceTypesApiResponse apiResponse,
        GetQualificationsApiResponse apiResponseQualifications,
        GetAddQualificationQuery query,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetAddQualificationQueryHandler handler
    )
    {
        query.Id = apiResponseQualifications.Qualifications.FirstOrDefault()!.Id;
        query.QualificationReferenceTypeId = apiResponse.QualificationReferences.Last().Id;
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetQualificationReferenceTypesApiResponse>(
                It.IsAny<GetQualificationReferenceTypesApiRequest>())).ReturnsAsync(
                new ApiResponse<GetQualificationReferenceTypesApiResponse>(apiResponse, HttpStatusCode.OK, ""));
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetQualificationsApiResponse>(
                It.Is<GetQualificationsApiRequest>(
                    c=>c.GetUrl.Contains(query.ApplicationId.ToString())
                       && c.GetUrl.Contains(query.CandidateId.ToString())
                       && c.GetUrl.Contains(query.QualificationReferenceTypeId.ToString())
                ))).ReturnsAsync(
                new ApiResponse<GetQualificationsApiResponse>(apiResponseQualifications, HttpStatusCode.OK, ""));
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.QualificationType.Should()
            .BeEquivalentTo(apiResponse.QualificationReferences.FirstOrDefault(c => c.Id == query.QualificationReferenceTypeId));
        actual.Qualifications.Should().BeEquivalentTo(new List<Qualification>
            { apiResponseQualifications.Qualifications.FirstOrDefault() });
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Request_To_CandidateApi_Is_Made_And_Courses_Returned_If_Single_Qualification_Returned_Is_Apprenticeship(
        GetQualificationReferenceTypesApiResponse apiResponse,
        GetQualificationsApiResponse apiResponseQualifications,
        GetAddQualificationQuery query,
        GetStandardsApiResponse standards,
        GetFrameworksApiResponse frameworks,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetAddQualificationQueryHandler handler
    )
    {
        apiResponse.QualificationReferences.Last().Name = "Apprenticeship";
        query.Id = apiResponseQualifications.Qualifications.FirstOrDefault()!.Id;
        query.QualificationReferenceTypeId = apiResponse.QualificationReferences.Last().Id;
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetQualificationReferenceTypesApiResponse>(
                It.IsAny<GetQualificationReferenceTypesApiRequest>())).ReturnsAsync(
                new ApiResponse<GetQualificationReferenceTypesApiResponse>(apiResponse, HttpStatusCode.OK, ""));
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetQualificationsApiResponse>(
                It.Is<GetQualificationsApiRequest>(
                    c=>c.GetUrl.Contains(query.ApplicationId.ToString())
                       && c.GetUrl.Contains(query.CandidateId.ToString())
                       && c.GetUrl.Contains(query.QualificationReferenceTypeId.ToString())
                ))).ReturnsAsync(
                new ApiResponse<GetQualificationsApiResponse>(apiResponseQualifications, HttpStatusCode.OK, ""));
        coursesApiClient.Setup(x => x.Get<GetStandardsApiResponse>(It.IsAny<GetAllStandardsRequest>()))
            .ReturnsAsync(standards);
        coursesApiClient.Setup(x => x.Get<GetFrameworksApiResponse>(It.IsAny<GetAllFrameworksApiRequest>()))
            .ReturnsAsync(frameworks);
        var expectedCourses = new List<GetAddQualificationQueryResult.CourseResponse>();
        expectedCourses.AddRange(standards.Standards.Select(standard => 
            new GetAddQualificationQueryResult.CourseResponse
            {
                Id = standard.StandardUId, 
                Title = standard.Title, 
                IsStandard = true
            }));
        expectedCourses.AddRange(frameworks.Frameworks.Select(framework => 
            new GetAddQualificationQueryResult.CourseResponse
            {
                Id = framework.Id, 
                Title = framework.Title, 
                IsStandard = false
            }));
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.QualificationType.Should()
            .BeEquivalentTo(apiResponse.QualificationReferences.FirstOrDefault(c => c.Id == query.QualificationReferenceTypeId));
        actual.Qualifications.Should().BeEquivalentTo(new List<Qualification>
            { apiResponseQualifications.Qualifications.FirstOrDefault() });
        actual.Courses.Should().BeEquivalentTo(expectedCourses);
        cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetStandardsApiResponse), It.IsAny<GetStandardsApiResponse>(), TimeSpan.FromHours(12)), Times.Once);
        cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetFrameworksApiResponse), It.IsAny<GetFrameworksApiResponse>(), TimeSpan.FromHours(12)), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Request_To_CandidateApi_Is_Made_And_Courses_Returned_From_Cache_If_Single_Qualification_Returned_Is_Apprenticeship(
        GetQualificationReferenceTypesApiResponse apiResponse,
        GetQualificationsApiResponse apiResponseQualifications,
        GetAddQualificationQuery query,
        GetStandardsApiResponse standards,
        GetFrameworksApiResponse frameworks,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetAddQualificationQueryHandler handler
    )
    {
        apiResponse.QualificationReferences.Last().Name = "Apprenticeship";
        query.Id = apiResponseQualifications.Qualifications.FirstOrDefault()!.Id;
        query.QualificationReferenceTypeId = apiResponse.QualificationReferences.Last().Id;
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetQualificationReferenceTypesApiResponse>(
                It.IsAny<GetQualificationReferenceTypesApiRequest>())).ReturnsAsync(
                new ApiResponse<GetQualificationReferenceTypesApiResponse>(apiResponse, HttpStatusCode.OK, ""));
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetQualificationsApiResponse>(
                It.Is<GetQualificationsApiRequest>(
                    c=>c.GetUrl.Contains(query.ApplicationId.ToString())
                       && c.GetUrl.Contains(query.CandidateId.ToString())
                       && c.GetUrl.Contains(query.QualificationReferenceTypeId.ToString())
                ))).ReturnsAsync(
                new ApiResponse<GetQualificationsApiResponse>(apiResponseQualifications, HttpStatusCode.OK, ""));
        cacheStorageService.Setup(x => x.RetrieveFromCache<GetStandardsApiResponse>(nameof(GetStandardsApiResponse)))
            .ReturnsAsync(standards);
        cacheStorageService.Setup(x => x.RetrieveFromCache<GetFrameworksApiResponse>(nameof(GetFrameworksApiResponse)))
            .ReturnsAsync(frameworks);
        var expectedCourses = new List<GetAddQualificationQueryResult.CourseResponse>();
        expectedCourses.AddRange(standards.Standards.Select(standard => 
            new GetAddQualificationQueryResult.CourseResponse
            {
                Id = standard.StandardUId, 
                Title = standard.Title, 
                IsStandard = true
            }));
        expectedCourses.AddRange(frameworks.Frameworks.Select(framework => 
            new GetAddQualificationQueryResult.CourseResponse
            {
                Id = framework.Id, 
                Title = framework.Title, 
                IsStandard = false
            }));
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.QualificationType.Should()
            .BeEquivalentTo(apiResponse.QualificationReferences.FirstOrDefault(c => c.Id == query.QualificationReferenceTypeId));
        actual.Qualifications.Should().BeEquivalentTo(new List<Qualification>
            { apiResponseQualifications.Qualifications.FirstOrDefault() });
        actual.Courses.Should().BeEquivalentTo(expectedCourses);
        coursesApiClient.Verify(x => x.Get<GetStandardsApiResponse>(It.IsAny<GetAllStandardsRequest>()), Times.Never);
        coursesApiClient.Verify(x => x.Get<GetFrameworksApiResponse>(It.IsAny<GetAllFrameworksApiRequest>()), Times.Never);
        
    }
}