using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteWorkExperience;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;
public class WhenHandlingDeleteVolunteeringOrWorkExperienceCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_VolunteeringOrWorkExperienceItem_Is_Deleted(
        PostDeleteVolunteeringOrWorkExperienceCommand command,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        PostDeleteVolunteeringOrWorkExperienceCommandHandler handler)
    {
        var expectedRequest = new PostDeleteJobApiRequest(command.ApplicationId, command.CandidateId, command.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().Be(Unit.Value);
            candidateApiClient
                .Verify(client => client.Delete(It.Is<PostDeleteJobApiRequest>(r => r.DeleteUrl == expectedRequest.DeleteUrl)), Times.Once);
        }
    }
}
