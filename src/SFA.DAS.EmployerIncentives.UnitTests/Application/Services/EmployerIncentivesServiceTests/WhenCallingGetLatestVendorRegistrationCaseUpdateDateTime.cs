using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingGetLatestVendorRegistrationCaseUpdateDateTime
    {
        [Test, MoqAutoData]
        public async Task Then_The_IncentivesApi_Is_Called_Returning_The_Response(
            GetLatestVendorRegistrationCaseUpdateDateTimeResponse apiResponse,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            VendorRegistrationService service
        )
        {
            client.Setup(x =>
                    x.Get<GetLatestVendorRegistrationCaseUpdateDateTimeResponse>(
                        It.IsAny<GetLatestVendorRegistrationCaseUpdateDateTimeRequest>()))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetLatestVendorRegistrationCaseUpdateDateTime();

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}