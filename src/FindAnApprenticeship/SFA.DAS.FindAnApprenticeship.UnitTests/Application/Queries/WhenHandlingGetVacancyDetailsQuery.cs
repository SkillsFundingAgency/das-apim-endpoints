using SFA.DAS.FindAnApprenticeship.Application.Queries.GetVacancyDetails;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries
{
    [TestFixture]
    public class WhenHandlingGetVacancyDetailsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Vacancy_Details_Returned(
            GetVacancyDetailsQuery query,
            GetApprenticeshipVacancyItemResponse getApprenticeshipVacancyItemResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
            GetVacancyDetailsQueryHandler handler)
        {
            findApprenticeshipApiClient.Setup(x => x.Get<GetApprenticeshipVacancyItemResponse>(
                    It.Is<GetVacancyRequest>(request =>
                        request.GetUrl.Contains(query.VacancyReference))))
                .ReturnsAsync(getApprenticeshipVacancyItemResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.ApprenticeshipVacancy.Should().BeEquivalentTo(getApprenticeshipVacancyItemResponse, options => options
                .Excluding(x =>x.ClosedDate)
                .Excluding(x => x.Postcode)
                .Excluding(x => x.City)
                .Excluding(x => x.IsExternalVacancy)
                .Excluding(x => x.ExternalVacancyUrl)
                .Excluding(x => x.Application)
                .Excluding(x => x.IsSavedVacancy)
            );
        }
    }
}
