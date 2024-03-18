using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetQualificationTypes;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;

public class WhenHandlingGetQualificationTypesQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_To_CandidateApi_Is_Made_And_Data_Returned(
        GetQualificationReferenceTypesApiResponse apiResponse,
        GetQualificationTypesQuery query,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetQualificationTypesQueryHandler handler
        )
    {
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetQualificationReferenceTypesApiResponse>(
                It.IsAny<GetQualificationReferenceTypesApiRequest>())).ReturnsAsync(
                new ApiResponse<GetQualificationReferenceTypesApiResponse>(apiResponse, HttpStatusCode.OK, ""));
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.QualificationTypes.Should().BeEquivalentTo(apiResponse.QualificationReferences);
    }
}