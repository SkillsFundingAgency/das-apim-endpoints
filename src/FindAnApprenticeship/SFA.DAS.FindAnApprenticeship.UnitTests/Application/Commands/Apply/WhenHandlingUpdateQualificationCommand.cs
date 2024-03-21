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
    public async Task Then_The_Command_Is_Handled_And_Api_Called_For_Each_Subject(
        UpdateApplicationQualificationCommand.Subject deleted,
        UpdateApplicationQualificationCommand command,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        UpdateApplicationQualificationCommandHandler handler)
    {
        deleted.IsDeleted = true;
        command.Subjects.Add(deleted);
        
        await handler.Handle(command, CancellationToken.None);

        foreach (var subject in command.Subjects.Where(x=>x.IsDeleted is null or false))
        {
            candidateApiClient.Verify(x =>
                x.PutWithResponseCode<PutApplicationQualificationApiResponse>(
                    It.Is<PutApplicationQualificationApiRequest>(c =>
                        c.PutUrl.Contains(
                            $"api/candidates/{command.CandidateId}/applications/{command.ApplicationId}/Qualifications")
                        && ((PutApplicationQualificationApiRequestData)c.Data).QualificationReferenceId == command.QualificationReferenceId
                        && ((PutApplicationQualificationApiRequestData)c.Data).Id == subject.Id
                        && ((PutApplicationQualificationApiRequestData)c.Data).ToYear == subject.ToYear
                        && ((PutApplicationQualificationApiRequestData)c.Data).Grade == subject.Grade
                        && ((PutApplicationQualificationApiRequestData)c.Data).Subject == subject.Name
                        && ((PutApplicationQualificationApiRequestData)c.Data).IsPredicted == subject.IsPredicted
                        && ((PutApplicationQualificationApiRequestData)c.Data).AdditionalInformation == subject.AdditionalInformation
                    )));
        }

        candidateApiClient.Verify(x =>
            x.Delete(It.Is<DeleteQualificationApiRequest>(c =>
                c.DeleteUrl.Contains(command.ApplicationId.ToString())
                && c.DeleteUrl.Contains(command.CandidateId.ToString())
                && c.DeleteUrl.Contains(deleted.Id.ToString())
                )), Times.Once);
    }
}