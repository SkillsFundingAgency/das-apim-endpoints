using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingGetIncentiveDetails
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_Incentive_Details(
            GetIncentiveDetailsResponse apiResponse,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service
        )
        {
            client.Setup(x =>
                    x.Get<GetIncentiveDetailsResponse>(
                        It.IsAny<GetIncentiveDetailsRequest>()))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetIncentiveDetails();

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}