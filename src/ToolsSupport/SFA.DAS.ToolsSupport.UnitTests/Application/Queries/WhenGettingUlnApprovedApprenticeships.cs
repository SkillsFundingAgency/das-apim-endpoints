using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using FluentAssertions;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;

public class WhenGettingUlnApprovedApprenticeships
{
    [Test, MoqAutoData]
    public async Task Then_Gets_CohortDetails_And_SupportStatus_And_Apprenticeships(
        string uln,
        GetApprovedApprenticeshipsByUlnResponse mockApiApprenticeshipsResponse,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetUlnSupportApprenticeshipsQueryHandler sut)
    {
        var mockUlnQuery = new GetUlnSupportApprenticeshipsQuery { Uln = uln };
        var expectedUrl = $"api/apprenticeships/uln/{uln}/approved-apprenticeships";
        mockApiClient.Setup(client => client.Get<GetApprovedApprenticeshipsByUlnResponse>(It.Is<GetApprovedApprenticeshipsByUlnRequest>(c => 
                c.GetUrl == expectedUrl)))
            .ReturnsAsync(mockApiApprenticeshipsResponse);


        var actual = await sut.Handle(mockUlnQuery, It.IsAny<CancellationToken>());

        actual.ApprovedApprenticeships.Should().BeEquivalentTo(
            mockApiApprenticeshipsResponse.ApprovedApprenticeships.Select(x=> 
                new
                {
                    x.Id,
                    x.FirstName,
                    x.LastName,
                    x.EmployerAccountId,
                    x.ProviderId,
                    x.EmployerName,
                    x.Uln,
                    x.StartDate,
                    x.EndDate,
                    Status = x.PaymentStatus.ToString()
                }));
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Empty_List_When_No_Matching_Uln(
        string uln,
        GetApprovedApprenticeshipsByUlnResponse mockApiApprenticeshipsResponse,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetUlnSupportApprenticeshipsQueryHandler sut)
    {
        mockApiApprenticeshipsResponse.ApprovedApprenticeships = new List<SupportApprenticeshipDetails>();
        var mockUlnQuery = new GetUlnSupportApprenticeshipsQuery { Uln = uln };
        var expectedUrl = $"api/apprenticeships/uln/{uln}/approved-apprenticeships";
        mockApiClient.Setup(client => client.Get<GetApprovedApprenticeshipsByUlnResponse>(It.IsAny<GetApprovedApprenticeshipsByUlnRequest>()))
            .ReturnsAsync(mockApiApprenticeshipsResponse);

        var actual = await sut.Handle(mockUlnQuery, It.IsAny<CancellationToken>());

        actual.ApprovedApprenticeships.Count().Should().Be(0);
    }
}