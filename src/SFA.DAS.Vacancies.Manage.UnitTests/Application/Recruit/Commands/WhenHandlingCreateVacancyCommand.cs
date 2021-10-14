using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Commands.CreateVacancy;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.UnitTests.Application.Recruit.Commands
{
    public class WhenHandlingCreateVacancyCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Api_Called_With_Response(
            string responseValue,
            CreateVacancyCommand command,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            var apiResponse = new ApiResponse<string>(responseValue, HttpStatusCode.Created, "");
            recruitApiClient.Setup(x =>
                x.PostWithResponseCode<string>(
                    It.Is<PostVacancyRequest>(c => 
                        c.PostUrl.Contains($"{command.Id.ToString()}?ukprn={command.PostVacancyRequestData.User.Ukprn}&userEmail={command.PostVacancyRequestData.User.Email}")
                        && ((PostVacancyRequestData)c.Data).Title.Equals(command.PostVacancyRequestData.Title)
                        )))
                .ReturnsAsync(apiResponse);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.VacancyReference.Should().Be(apiResponse.Body);
        }

        [Test, MoqAutoData]
        public void Then_If_There_Is_A_Bad_Request_Error_From_The_Api_A_Exception_Is_Returned(
            string errorContent,
            CreateVacancyCommand command,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            var apiResponse = new ApiResponse<string>(null, HttpStatusCode.BadRequest, errorContent);
            recruitApiClient
                .Setup(client => client.PostWithResponseCode<string>(It.IsAny<PostVacancyRequest>()))
                .ReturnsAsync(apiResponse);
            
            //Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            
            //Assert
            act.Should().Throw<HttpRequestContentException>().WithMessage($"Response status code does not indicate success: {(int)HttpStatusCode.BadRequest} ({HttpStatusCode.BadRequest})")
                .Which.ErrorContent.Should().Be(errorContent);
        }
        [Test, MoqAutoData]
        public void Then_If_There_Is_An_Error_From_The_Api_A_Exception_Is_Returned(
            string errorContent,
            CreateVacancyCommand command,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            CreateVacancyCommandHandler handler)
        {
            //Arrange
            var apiResponse = new ApiResponse<string>(null, HttpStatusCode.InternalServerError, errorContent);
            recruitApiClient
                .Setup(client => client.PostWithResponseCode<string>(It.IsAny<PostVacancyRequest>()))
                .ReturnsAsync(apiResponse);
            
            //Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            
            //Assert
            act.Should().Throw<Exception>()
                .WithMessage($"Response status code does not indicate success: {(int)HttpStatusCode.InternalServerError} ({HttpStatusCode.InternalServerError})");
        }
    }
}