using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeshipsManage.Application.Queries.GetApprenticeships;
using SFA.DAS.ApprenticeshipsManage.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeshipsManage.Tests.Application.Queries;
public class GetApprenticeshipsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Users_With_Matching_Emails(
        GetApprenticeshipsQuery query,
        PagedApprenticeshipsResponse apiResponse,
        [Frozen] Mock<ILearningApiClient<LearningApiConfiguration>> apiClient,
        GetApprenticeshipsQueryHandler sut)
    {

        query.AcademicYear = 2425;

        var expectedUrl = $"/{query.Ukprn}/academicyears/{query.AcademicYear}/learnings?page={query.Page}&pageSize={query.PageSize}";

        apiClient.Setup(client => client.Get<PagedApprenticeshipsResponse>(It.Is<GetAllLearningsRequest>(c => c.GetUrl == expectedUrl)))
            .ReturnsAsync(apiResponse);

        var actual = await sut.Handle(query, It.IsAny<CancellationToken>());

        actual.Items.Should().BeEquivalentTo(apiResponse.Items);
        actual.TotalItems.Should().Be(apiResponse.TotalItems);
        actual.TotalPages.Should().Be((int)Math.Ceiling((double)apiResponse.TotalItems / apiResponse.PageSize));
        actual.Page.Should().Be(apiResponse.Page);
        actual.PageSize.Should().Be(apiResponse.PageSize);
    }
}