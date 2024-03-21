using SFA.DAS.Testing.AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteQualifications;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply.Qualifications
{
    public class WhenHandlingDeleteQualificationsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_Qualifications_Of_The_Given_Type_Are_Deleted(
            DeleteQualificationsCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            DeleteQualificationsCommandHandler handler)
        {
            var expectedRequest = new DeleteQualificationsByTypeApiRequest(command.ApplicationId, command.CandidateId, command.QualificationReferenceId);

            await handler.Handle(command, CancellationToken.None);

            candidateApiClient
                .Verify(client => client.Delete(It.Is<DeleteQualificationsByTypeApiRequest>(r => r.DeleteUrl == expectedRequest.DeleteUrl)), Times.Once);
        }
    }
}
