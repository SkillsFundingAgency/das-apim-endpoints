using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Queries.GetQualifications;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.UnitTests.Application.Recruit.Queries
{
    public class WhenHandlingQualificationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Api_Called(
            GetQualificationsQueryResponse apiQueryResponse,
            GetQualificationsQuery query,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> apiClient,
          GetQualificationsQueryHandler handler)
        {
            apiClient.Setup(x =>
                    x.Get<GetQualificationsQueryResponse>(
                        It.Is<GetQualificationsRequest>(c => c.GetUrl.Contains($"referencedata/candidate-qualifications"))))
                .ReturnsAsync(apiQueryResponse);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Qualifications.Should().BeEquivalentTo(apiQueryResponse.Qualifications);
        }
    }
}