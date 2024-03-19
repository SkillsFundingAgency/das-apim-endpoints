using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateApplicationQualification;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;

public class WhenHandlingUpdateQualificationCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Api_Called(
        UpdateApplicationQualificationCommand command,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        UpdateApplicationQualificationCommandHandler handler)
    {
        await handler.Handle(command, CancellationToken.None);

        candidateApiClient.Verify(x =>
            x.PutWithResponseCode<PutApplicationQualificationApiResponse>(
                It.Is<PutApplicationQualificationApiRequest>(c =>
                    c.PutUrl.Contains(
                        $"api/candidates/{command.CandidateId}/applications/{command.ApplicationId}/Qualifications")
                    && ((PutApplicationQualificationApiRequestData)c.Data).QualificationReferenceId == command.QualificationReferenceId
                    && ((PutApplicationQualificationApiRequestData)c.Data).Id == command.Id
                    && ((PutApplicationQualificationApiRequestData)c.Data).ToYear == command.ToYear
                    && ((PutApplicationQualificationApiRequestData)c.Data).Grade == command.Grade
                    && ((PutApplicationQualificationApiRequestData)c.Data).Subject == command.Subject
                    && ((PutApplicationQualificationApiRequestData)c.Data).IsPredicted == command.IsPredicted
                    && ((PutApplicationQualificationApiRequestData)c.Data).AdditionalInformation == command.AdditionalInformation
                    )));
    }
}