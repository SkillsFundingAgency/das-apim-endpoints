using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetIndexQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_VacancyDetails_Are_Returned_As_Expected(
            GetIndexQuery query,
            GetApprenticeshipVacancyItemResponse apiResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetIndexQueryHandler handler)
        {
            var expectedRequest = new GetVacancyRequest(query.VacancyReference);

            apiClient
                .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(It.Is<GetVacancyRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(new
            {
                VacancyTitle = apiResponse.Title,
                EmployerName = apiResponse.EmployerName,
                ClosingDate = apiResponse.ClosingDate,
                IsDisabilityConfident = apiResponse.IsDisabilityConfident
            });
        }
    }
}
