using SFA.DAS.FindApprenticeshipJobs.Application.Commands;
using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using SFA.DAS.FindApprenticeshipJobs.Domain.Constants;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;

public class WhenHandlingProcessVacancyClosedEarlyCommand
{
    [Test]
    [MoqInlineAutoData("address1", "address2", "address3", "address4", "postcode", "address4 (postcode)", AvailableWhere.OneLocation)]
    [MoqInlineAutoData("address1", "address2", "address3", null, "postcode", "address3 (postcode)", AvailableWhere.OneLocation)]
    [MoqInlineAutoData("address1", "address2", null, null, "postcode", "address2 (postcode)", AvailableWhere.OneLocation)]
    [MoqInlineAutoData("address1", null, null, null, "postcode", "address1 (postcode)", AvailableWhere.OneLocation)]
    [MoqInlineAutoData("address1", "address2", "address3", "address4", "postcode", EmailTemplateBuilderConstants.RecruitingNationally, AvailableWhere.AcrossEngland)]
    [MoqInlineAutoData("address1", "address2", "address3", null, "postcode", EmailTemplateBuilderConstants.RecruitingNationally, AvailableWhere.AcrossEngland)]
    [MoqInlineAutoData("address1", "address2", null, null, "postcode", EmailTemplateBuilderConstants.RecruitingNationally, AvailableWhere.AcrossEngland)]
    [MoqInlineAutoData("address1", null, null, null, "postcode", EmailTemplateBuilderConstants.RecruitingNationally, AvailableWhere.AcrossEngland)]
    public async Task Then_The_Vacancy_Candidates_Are_Found_Emails_Sent_And_Application_Status_Updated(
        string address1,
        string address2,
        string address3,
        string address4,
        string postcode,
        string expectedAddress,
        AvailableWhere employerLocationOption,
        ProcessVacancyClosedEarlyCommand command,
        GetCandidateApplicationApiResponse candidateApiResponseAll,
        GetClosedVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        recruitApiResponse.EmployerLocationOption = employerLocationOption;

        recruitApiResponse.Address.AddressLine1 = address1;
        recruitApiResponse.Address.AddressLine2 = address2;
        recruitApiResponse.Address.AddressLine3 = address3;
        recruitApiResponse.Address.AddressLine4 = address4;
        recruitApiResponse.Address.Postcode = postcode;

        candidateApiClient.Setup(x => 
                x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));
        
        var candidateGetRequestAll =
            new GetCandidateApplicationsByVacancyRequest(command.VacancyReference.ToString(), null,
                false);
       candidateApiClient
            .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                    c.GetUrl == candidateGetRequestAll.GetUrl))).ReturnsAsync(candidateApiResponseAll);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetClosedVacancyApiResponse>(
                It.Is<GetClosedVacancyApiRequest>(c => 
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(new ApiResponse<GetClosedVacancyApiResponse>(recruitApiResponse, HttpStatusCode.OK, ""));

        await handler.Handle(command, CancellationToken.None);

        foreach (var candidate in candidateApiResponseAll.Candidates)
        {
            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                    c.PatchUrl.Contains(candidate.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.PatchUrl.Contains(candidate.Candidate.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.Data.Operations[0].path == "/Status" &&
                    (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Expired
                )), Times.Once
                
            );
        }
    }
    
    [Test,MoqAutoData]
    public async Task Then_The_Get_Closed_Vacancy_Request_Is_Retried_If_NotFound_Candidates_Are_Found_Emails_Sent_And_Application_Status_Updated(
        ProcessVacancyClosedEarlyCommand command,
        GetCandidateApplicationApiResponse candidateApiResponseAll,
        GetClosedVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        recruitApiResponse.EmployerLocationOption = AvailableWhere.OneLocation;
        recruitApiResponse.Address.AddressLine1 = "address1";
        recruitApiResponse.Address.AddressLine2 = null;
        recruitApiResponse.Address.AddressLine3 = null;
        recruitApiResponse.Address.AddressLine4 = null;
        
        candidateApiClient.Setup(x => 
                x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));
        
        var candidateGetRequestAll =
            new GetCandidateApplicationsByVacancyRequest(command.VacancyReference.ToString(), null,
                false);
       candidateApiClient
            .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                    c.GetUrl == candidateGetRequestAll.GetUrl))).ReturnsAsync(candidateApiResponseAll);
        recruitApiClient
            .SetupSequence(x => x.GetWithResponseCode<GetClosedVacancyApiResponse>(
                It.Is<GetClosedVacancyApiRequest>(c => 
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(new ApiResponse<GetClosedVacancyApiResponse>(null!, HttpStatusCode.NotFound, ""))
            .ReturnsAsync(new ApiResponse<GetClosedVacancyApiResponse>(recruitApiResponse, HttpStatusCode.OK, ""));

        await handler.Handle(command, CancellationToken.None);

        foreach (var candidate in candidateApiResponseAll.Candidates)
        {
            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                    c.PatchUrl.Contains(candidate.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.PatchUrl.Contains(candidate.Candidate.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.Data.Operations[0].path == "/Status" &&
                    (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Expired
                )), Times.Once
                
            );
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_EmployerLocation_Null_Candidates_Are_Found_Emails_Sent_And_Application_Status_Updated(
        ProcessVacancyClosedEarlyCommand command,
        GetCandidateApplicationApiResponse candidateApiResponseAll,
        GetClosedVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        const string expectedAddress = "city (postcode)";

        recruitApiResponse.EmployerLocationOption = null;

        recruitApiResponse.Address!.AddressLine1 = "address1";
        recruitApiResponse.Address.AddressLine2 = "address2";
        recruitApiResponse.Address.AddressLine3 = "address3";
        recruitApiResponse.Address.AddressLine4 = "city";
        recruitApiResponse.Address.Postcode = "postcode";

        candidateApiClient.Setup(x =>
                x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));

        var candidateGetRequestAll =
            new GetCandidateApplicationsByVacancyRequest(command.VacancyReference.ToString(), null,
                false);
        candidateApiClient
             .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                 It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                     c.GetUrl == candidateGetRequestAll.GetUrl))).ReturnsAsync(candidateApiResponseAll);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetClosedVacancyApiResponse>(
                It.Is<GetClosedVacancyApiRequest>(c =>
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(new ApiResponse<GetClosedVacancyApiResponse>(recruitApiResponse, HttpStatusCode.OK,""));

        await handler.Handle(command, CancellationToken.None);

        foreach (var candidate in candidateApiResponseAll.Candidates)
        {
            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                    c.PatchUrl.Contains(candidate.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.PatchUrl.Contains(candidate.Candidate.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.Data.Operations[0].path == "/Status" &&
                    (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Expired
                )), Times.Once

            );
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_With_Multiple_Locations_And_Candidates_Are_Found_Emails_Sent_And_Application_Status_Updated(
        List<Address> addresses,
        ProcessVacancyClosedEarlyCommand command,
        GetCandidateApplicationApiResponse candidateApiResponseAll,
        GetClosedVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        recruitApiResponse.EmployerLocationOption = AvailableWhere.MultipleLocations;
        recruitApiResponse.EmployerLocations = addresses;

        candidateApiClient.Setup(x =>
                x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));

        var candidateGetRequestAll =
            new GetCandidateApplicationsByVacancyRequest(command.VacancyReference.ToString(), null,
                false);
        candidateApiClient
             .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                 It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                     c.GetUrl == candidateGetRequestAll.GetUrl))).ReturnsAsync(candidateApiResponseAll);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetClosedVacancyApiResponse>(
                It.Is<GetClosedVacancyApiRequest>(c =>
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(new ApiResponse<GetClosedVacancyApiResponse>(recruitApiResponse, HttpStatusCode.OK, ""));

        await handler.Handle(command, CancellationToken.None);

        foreach (var candidate in candidateApiResponseAll.Candidates)
        {
            var employmentWorkLocation =
                EmailTemplateAddressExtension.GetEmploymentLocationCityNames(recruitApiResponse.OtherAddresses);

            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                    c.PatchUrl.Contains(candidate.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.PatchUrl.Contains(candidate.Candidate.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.Data.Operations[0].path == "/Status" &&
                    (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Expired
                )), Times.Once

            );
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_With_Anon_Multiple_Locations_And_Candidates_Are_Found_Emails_Sent_And_Application_Status_Updated(
        ProcessVacancyClosedEarlyCommand command,
        GetCandidateApplicationApiResponse candidateApiResponseAll,
        GetClosedVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        const string expectedAddress = "Leeds (3 available locations)";

        recruitApiResponse.EmployerLocationOption = AvailableWhere.MultipleLocations;
        recruitApiResponse.EmployerLocations =
        [
            new Address {AddressLine3 = "Leeds", Postcode = "LS6"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS6"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS16"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS9"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS9"}
        ];

        candidateApiClient.Setup(x =>
                x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));

        var candidateGetRequestAll =
            new GetCandidateApplicationsByVacancyRequest(command.VacancyReference.ToString(), null,
                false);
        candidateApiClient
             .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                 It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                     c.GetUrl == candidateGetRequestAll.GetUrl))).ReturnsAsync(candidateApiResponseAll);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetClosedVacancyApiResponse>(
                It.Is<GetClosedVacancyApiRequest>(c =>
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(new ApiResponse<GetClosedVacancyApiResponse>(recruitApiResponse, HttpStatusCode.OK, ""));

        await handler.Handle(command, CancellationToken.None);

        foreach (var candidate in candidateApiResponseAll.Candidates)
        {
            var employmentWorkLocation =
                EmailTemplateAddressExtension.GetEmploymentLocationCityNames(recruitApiResponse.OtherAddresses);

            employmentWorkLocation.Should().Be(expectedAddress);

            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                    c.PatchUrl.Contains(candidate.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.PatchUrl.Contains(candidate.Candidate.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.Data.Operations[0].path == "/Status" &&
                    (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Expired
                )), Times.Once

            );
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_With_Multiple_Locations_And_Candidates_Are_Found_Emails_Sent_And_Application_Status_Updated(
        ProcessVacancyClosedEarlyCommand command,
        GetCandidateApplicationApiResponse candidateApiResponseAll,
        GetClosedVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        const string expectedAddress = "Leeds, Manchester, Sheffield";

        recruitApiResponse.EmployerLocationOption = AvailableWhere.MultipleLocations;
        recruitApiResponse.EmployerLocations =
        [
            new Address {AddressLine3 = "Leeds", Postcode = "LS6 8AA"},
            new Address {AddressLine3 = "Manchester", Postcode = "MA1 1AN"},
            new Address {AddressLine3 = "Sheffield", Postcode = "SF1 1AN"},
            new Address {AddressLine3 = "Sheffield", Postcode = "SF1 1AN"},
            new Address {AddressLine3 = "Manchester", Postcode = "MA1 1AN"},
        ];

        candidateApiClient.Setup(x =>
                x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));

        var candidateGetRequestAll =
            new GetCandidateApplicationsByVacancyRequest(command.VacancyReference.ToString(), null,
                false);
        candidateApiClient
             .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                 It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                     c.GetUrl == candidateGetRequestAll.GetUrl))).ReturnsAsync(candidateApiResponseAll);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetClosedVacancyApiResponse>(
                It.Is<GetClosedVacancyApiRequest>(c =>
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(new ApiResponse<GetClosedVacancyApiResponse>(recruitApiResponse, HttpStatusCode.OK, ""));

        await handler.Handle(command, CancellationToken.None);

        foreach (var candidate in candidateApiResponseAll.Candidates)
        {
            var employmentWorkLocation =
                EmailTemplateAddressExtension.GetEmploymentLocationCityNames(recruitApiResponse.OtherAddresses);

            employmentWorkLocation.Should().Be(expectedAddress);

            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                    c.PatchUrl.Contains(candidate.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.PatchUrl.Contains(candidate.Candidate.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.Data.Operations[0].path == "/Status" &&
                    (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Expired
                )), Times.Once

            );
        }
    }

    [Test, MoqAutoData]
    public async Task Then_If_Cannot_Find_Vacancy_Exception_Is_Thrown(
        ProcessVacancyClosedEarlyCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        candidateApiClient
            .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                    c.GetUrl.Contains(command.VacancyReference.ToString())
                    ))).ReturnsAsync(new GetCandidateApplicationApiResponse{Candidates = []});
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetClosedVacancyApiResponse>(
                It.Is<GetClosedVacancyApiRequest>(c => 
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(new ApiResponse<GetClosedVacancyApiResponse>(null!, HttpStatusCode.NotFound, ""));

        Assert.ThrowsAsync<Exception>(()=> handler.Handle(command, CancellationToken.None));
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_No_Candidates_Are_Found_No_Emails_Sent(
        ProcessVacancyClosedEarlyCommand command,
        GetClosedVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        candidateApiClient
            .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                    c.GetUrl.Contains(command.VacancyReference.ToString())
                ))).ReturnsAsync(new GetCandidateApplicationApiResponse{Candidates = []});
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetClosedVacancyApiResponse>(
                It.Is<GetClosedVacancyApiRequest>(c => 
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(new ApiResponse<GetClosedVacancyApiResponse>(recruitApiResponse, HttpStatusCode.OK, ""));

        await handler.Handle(command, CancellationToken.None);

        candidateApiClient.Verify(x => 
            x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never);
        
    }
}