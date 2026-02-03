using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Requests;
using SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.EmploymentCheck;

public class WhenGettingLearners
{
    [Test, MoqAutoData]
    public async Task Then_Returns_200_And_EvsCheck_List_When_Valid_Ids(
        List<long> apprenticeshipIds,
        List<EvsCheckResponse> evsChecks,
        [Frozen] Mock<IEmploymentCheckApiClient<EmploymentCheckConfiguration>> client,
        [Greedy] EmploymentChecksController controller)
    {
        client
            .Setup(x => x.GetWithResponseCode<List<EvsCheckResponse>>(It.IsAny<GetEmploymentCheckLearnersRequest>()))
            .ReturnsAsync(new ApiResponse<List<EvsCheckResponse>>(evsChecks, HttpStatusCode.OK, null));

        var result = await controller.Get(apprenticeshipIds);

        result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result;
        ok.Value.Should().BeEquivalentTo(evsChecks);
    }

    [Test, MoqAutoData]
    public async Task And_Null_ApprenticeshipIds_Then_Returns_400(
        [Greedy] EmploymentChecksController controller)
    {
        var result = await controller.Get(null);

        result.Should().BeOfType<BadRequestObjectResult>();
        ((BadRequestObjectResult)result).Value.Should().Be("apprenticeshipIds is required and must not be empty.");
    }

    [Test, MoqAutoData]
    public async Task And_Empty_ApprenticeshipIds_Then_Returns_400(
        [Greedy] EmploymentChecksController controller)
    {
        var result = await controller.Get(new List<long>());

        result.Should().BeOfType<BadRequestObjectResult>();
        ((BadRequestObjectResult)result).Value.Should().Be("apprenticeshipIds is required and must not be empty.");
    }

    [Test, MoqAutoData]
    public async Task And_More_Than_1000_Ids_Then_Returns_400(
        [Greedy] EmploymentChecksController controller)
    {
        var tooMany = new List<long>();
        for (long i = 0; i < 1001; i++) tooMany.Add(i);

        var result = await controller.Get(tooMany);

        result.Should().BeOfType<BadRequestObjectResult>();
        ((BadRequestObjectResult)result).Value.Should().Be("apprenticeshipIds must not exceed 1000.");
    }

    [Test, MoqAutoData]
    public async Task And_Inner_Api_Returns_Error_Then_Propagates_Status(
        List<long> apprenticeshipIds,
        [Frozen] Mock<IEmploymentCheckApiClient<EmploymentCheckConfiguration>> client,
        [Greedy] EmploymentChecksController controller)
    {
        client
            .Setup(x => x.GetWithResponseCode<List<EvsCheckResponse>>(It.IsAny<GetEmploymentCheckLearnersRequest>()))
            .ReturnsAsync(new ApiResponse<List<EvsCheckResponse>>(null, HttpStatusCode.InternalServerError, "Inner API error"));

        var result = await controller.Get(apprenticeshipIds);

        result.Should().BeOfType<ObjectResult>();
        var obj = (ObjectResult)result;
        obj.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        obj.Value.Should().Be("Inner API error");
    }
}
