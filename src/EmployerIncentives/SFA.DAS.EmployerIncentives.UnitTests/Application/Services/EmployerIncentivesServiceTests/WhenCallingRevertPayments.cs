using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    [TestFixture]
    public class WhenCallingRevertPayments
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Revert_Payments_Request(
            RevertPaymentsRequest request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service
        )
        {
            client.Setup(x => x.PostWithResponseCode<PostRevertPaymentsRequest>(
                It.Is<PostRevertPaymentsRequest>(
                    c => c.PostUrl.Contains("revert-payments")
                ), false)).ReturnsAsync(new ApiResponse<PostRevertPaymentsRequest>(null, HttpStatusCode.NoContent, ""));

            await service.RevertPayments(request);

            client.Verify(x => x.PostWithResponseCode<PostRevertPaymentsRequest>(
                It.Is<PostRevertPaymentsRequest>(c => c.PostUrl.Contains("revert-payments")
                ), false), Times.Once);
        }

        [Test]
        public async Task Then_an_exception_is_thrown_for_a_non_success_http_status_code()
        {
            // Arrange
            var fixture = new Fixture();
            var client = new Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>>();
            client.Setup(x => x.PostWithResponseCode<PostRevertPaymentsRequest>(
                It.Is<PostRevertPaymentsRequest>(
                    c => c.PostUrl.Contains("revert-payments")
                ), false)).ReturnsAsync(new ApiResponse<PostRevertPaymentsRequest>(null, HttpStatusCode.BadRequest, "Invalid request"));

            var request = fixture.Create<RevertPaymentsRequest>();

            var service = new EmployerIncentivesService(client.Object);

            // Act
            Func<Task> act = async () => { await service.RevertPayments(request); };

            // Assert
            await act.Should().ThrowAsync<HttpRequestContentException>();
        }
    }
}
