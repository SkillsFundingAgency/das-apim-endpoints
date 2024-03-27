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
            command.Id = null;
            var expectedRequest = new DeleteQualificationsByTypeApiRequest(command.ApplicationId, command.CandidateId, command.QualificationReferenceId);

            await handler.Handle(command, CancellationToken.None);

            candidateApiClient
                .Verify(client => client.Delete(It.Is<DeleteQualificationsByTypeApiRequest>(r => r.DeleteUrl == expectedRequest.DeleteUrl)), Times.Once);
        }
        [Test, MoqAutoData]
        public async Task Then_Qualifications_Are_Deleted_By_Id(
            DeleteQualificationsCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            DeleteQualificationsCommandHandler handler)
        {
            command.Id = Guid.NewGuid();
            var expectedRequest = new DeleteQualificationApiRequest(command.CandidateId, command.ApplicationId, command.Id.Value);

            await handler.Handle(command, CancellationToken.None);

            candidateApiClient
                .Verify(client => client.Delete(It.Is<DeleteQualificationApiRequest>(r => r.DeleteUrl == expectedRequest.DeleteUrl)), Times.Once);
        }
    }
}
