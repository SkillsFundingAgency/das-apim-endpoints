using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
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
}