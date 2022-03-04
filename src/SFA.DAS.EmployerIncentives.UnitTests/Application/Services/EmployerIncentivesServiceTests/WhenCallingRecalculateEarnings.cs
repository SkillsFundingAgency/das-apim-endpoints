using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings;
using SFA.DAS.EmployerIncentives.Interfaces;
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
                    c => c.PostUrl.Contains("recalculateEarnings")
                ))).ReturnsAsync(new ApiResponse<PostRecalculateEarningsRequest>(null, HttpStatusCode.NoContent, ""));

            await service.RecalculateEarnings(request);

            client.Verify(x => x.PostWithResponseCode<PostRecalculateEarningsRequest>(
                It.Is<PostRecalculateEarningsRequest>(c => c.PostUrl.Contains("recalculateEarnings")
                )), Times.Once);
        }
    }
}
