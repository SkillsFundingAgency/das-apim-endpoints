using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    [TestFixture]
    public class WhenCallingRecalculateEarnings
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Recalculate_Earnings_Request(
            RecalculateEarningsRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service
            )
        {
            client.Setup(x => x.PostWithResponseCode<PostRecalculateEarningsRequest>(
                It.Is<PostRecalculateEarningsRequest>(
                    c => c.PostUrl.Contains("earningsRecalculations")
                ))).ReturnsAsync(new ApiResponse<PostRecalculateEarningsRequest>(null, HttpStatusCode.NoContent, ""));

            await service.RecalculateEarnings(request);

            client.Verify(x => x.PostWithResponseCode<PostRecalculateEarningsRequest>(
                It.Is<PostRecalculateEarningsRequest>(c => c.PostUrl.Contains("earningsRecalculations")
                )), Times.Once);
        }

        [Test]
        public async Task Then_an_exception_is_thrown_for_a_non_success_http_status_code()
        {
            // Arrange
            var fixture = new Fixture();
            var client = new Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>>();
            client.Setup(x => x.PostWithResponseCode<PostRecalculateEarningsRequest>(
                It.Is<PostRecalculateEarningsRequest>(
                    c => c.PostUrl.Contains("earningsRecalculations")
                ))).ReturnsAsync(new ApiResponse<PostRecalculateEarningsRequest>(null, HttpStatusCode.BadRequest, "Invalid request"));

            var request = fixture.Create<RecalculateEarningsRequest>();

            var service = new EmployerIncentivesService(client.Object);

            // Act
            Func<Task> act = async () => { await service.RecalculateEarnings(request); };
            
            // Assert
            await act.Should().ThrowAsync<HttpRequestContentException>();
        }
    }
}
