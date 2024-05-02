using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Candidates.Commands.CandidateApplicationStatus;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.UnitTests.Application.Candidates.Commands;

public class WhenHandlingCandidateApplicationStatusCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_The_Api_Called(
        CandidateApplicationStatusCommand request,
        [Frozen]Mock<ICandidateApiClient<CandidateApiConfiguration>> apiClient,
        CandidateApplicationStatusCommandHandler handler)
    {
        apiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));
        
        await handler.Handle(request, CancellationToken.None);
        
        apiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/Feedback" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Outcome" &&
                c.Data.Operations[1].value.ToString() == request.Outcome
            )), Times.Once
        );
    }
}