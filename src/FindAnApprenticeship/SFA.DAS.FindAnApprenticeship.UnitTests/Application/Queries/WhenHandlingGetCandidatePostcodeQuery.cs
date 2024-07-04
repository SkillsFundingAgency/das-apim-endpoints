using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePostcode;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;

public class WhenHandlingGetCandidatePostcodeQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_Postcode_Returned(
        GetCandidatePostcodeQuery query,
        GetCandidateAddressApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetCandidatePostcodeQueryHandler handler)
    {
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetCandidateAddressApiResponse>(
                It.Is<GetCandidateAddressApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(new ApiResponse<GetCandidateAddressApiResponse>(apiResponse, HttpStatusCode.OK, ""));
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Postcode.Should().Be(apiResponse.Postcode);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_Null_Returned_If_Postcode_Not_Found(
        GetCandidatePostcodeQuery query,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetCandidatePostcodeQueryHandler handler)
    {
        candidateApiClient
            .Setup(x => x.GetWithResponseCode<GetCandidateAddressApiResponse>(
                It.Is<GetCandidateAddressApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(new ApiResponse<GetCandidateAddressApiResponse>(null, HttpStatusCode.NotFound, ""));
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Postcode.Should().BeNull();
    }
    
}