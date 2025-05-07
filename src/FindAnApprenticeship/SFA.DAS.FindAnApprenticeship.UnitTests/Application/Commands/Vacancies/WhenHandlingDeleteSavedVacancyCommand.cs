using AutoFixture.NUnit3;
using Azure.Core;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.DeleteSavedVacancy;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Vacancies
{
    [TestFixture]
    public class WhenHandlingDeleteSavedVacancyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Vacancy_Is_Deleted(
            DeleteSavedVacancyCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            DeleteSavedVacancyCommandHandler handler)
        {
            var expectedRequest = new PostDeleteSavedVacancyApiRequest(command.CandidateId, command.VacancyId.TrimVacancyReference(), command.DeleteAllByVacancyReference);

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().NotBeNull();
            actual.Should().Be(Unit.Value);
            candidateApiClient
                .Verify(client => client.Delete(It.Is<PostDeleteSavedVacancyApiRequest>(r => r.DeleteUrl == expectedRequest.DeleteUrl)), Times.Once);
        }
    }
}
