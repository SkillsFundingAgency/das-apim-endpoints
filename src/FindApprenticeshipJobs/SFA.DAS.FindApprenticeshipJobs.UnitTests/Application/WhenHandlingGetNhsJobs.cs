using System.Net;
using System.Xml;
using System.Xml.Serialization;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using LiveVacancy = SFA.DAS.FindApprenticeshipJobs.Application.Shared.LiveVacancy;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;

public class WhenHandlingGetNhsJobs
{
    [Test, MoqAutoData]
    public async Task Then_The_Nhs_Jobs_Are_Returned_From_The_Api_For_Multiple_Pages(
        GetNhsJobsQuery query,
        LiveVacancy liveVacancy,
        LiveVacancy liveVacancy2,
        LiveVacancy liveVacancy3,
        LiveVacancy liveVacancy4,
        [Frozen] Mock<ILiveVacancyMapper> mapper,
        [Frozen] Mock<INhsJobsApiClient> client,
        GetNhsJobsQueryHandler handler)
    {
        var xmlSerializer = new XmlSerializer(typeof(GetNhsJobApiResponse));
        object result;
        using (TextReader reader = new StringReader(_nhsResponse))
        {
            result = xmlSerializer.Deserialize(reader);
        }
        var expectedResponse = (GetNhsJobApiResponse)result!;
        object result2;
        using (TextReader reader = new StringReader(_nhsResponse2))
        {
            result2 = xmlSerializer.Deserialize(reader);
        }
        var expectedResponse2 = (GetNhsJobApiResponse)result2!;
        mapper.Setup(x => x.Map(It.IsAny<GetNhsJobApiDetailResponse>())).Returns((LiveVacancy)null);
        mapper.Setup(x => x.Map(It.Is<GetNhsJobApiDetailResponse>(c=>c.Id == expectedResponse.Vacancies.FirstOrDefault().Id))).Returns(liveVacancy);
        mapper.Setup(x => x.Map(It.Is<GetNhsJobApiDetailResponse>(c=>c.Id == expectedResponse.Vacancies.LastOrDefault().Id))).Returns(liveVacancy2);
        mapper.Setup(x => x.Map(It.Is<GetNhsJobApiDetailResponse>(c=>c.Id == expectedResponse2.Vacancies.FirstOrDefault().Id))).Returns(liveVacancy3);
        mapper.Setup(x => x.Map(It.Is<GetNhsJobApiDetailResponse>(c=>c.Id == expectedResponse2.Vacancies.LastOrDefault().Id))).Returns(liveVacancy4);
        client.Setup(x => x.GetWithResponseCode(It.Is<GetNhsJobsApiRequest>(c => c.GetUrl.Contains("?contractType=Apprenticeship&page=1"))))
            .ReturnsAsync(new ApiResponse<string>(_nhsResponse, HttpStatusCode.OK, ""));
        client.Setup(x => x.GetWithResponseCode(It.Is<GetNhsJobsApiRequest>(c => c.GetUrl.Contains("?contractType=Apprenticeship&page=2"))))
            .ReturnsAsync(new ApiResponse<string>(_nhsResponse2, HttpStatusCode.OK, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.NhsVacancies.Should().BeEquivalentTo(new List<LiveVacancy>{liveVacancy, liveVacancy2,liveVacancy3, liveVacancy4}.ToList());
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_There_Are_No_Nhs_Jobs_Returned_Then_Empty_List_Returned(
        GetNhsJobsQuery query,
        [Frozen] Mock<ILiveVacancyMapper> mapper,
        [Frozen] Mock<INhsJobsApiClient> client,
        GetNhsJobsQueryHandler handler)
    {
        mapper.Setup(x => x.Map(It.IsAny<GetNhsJobApiDetailResponse>())).Returns(((LiveVacancy)null!)!);
        client.Setup(x => x.GetWithResponseCode(It.Is<GetNhsJobsApiRequest>(c => c.GetUrl.Contains("?contractType=Apprenticeship&page=1"))))
            .ReturnsAsync(new ApiResponse<string>(_nhsNoResultsResponse, HttpStatusCode.OK, ""));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.NhsVacancies.Should().BeEquivalentTo(new List<LiveVacancy>());
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_There_Is_An_Error_Then_Empty_List_Returned(
        GetNhsJobsQuery query,
        [Frozen] Mock<ILiveVacancyMapper> mapper,
        [Frozen] Mock<INhsJobsApiClient> client,
        GetNhsJobsQueryHandler handler)
    {
        mapper.Setup(x => x.Map(It.IsAny<GetNhsJobApiDetailResponse>())).Returns(((LiveVacancy)null!)!);
        client.Setup(x => x.GetWithResponseCode(It.Is<GetNhsJobsApiRequest>(c => c.GetUrl.Contains("?contractType=Apprenticeship&page=1"))))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.InternalServerError, "An error"));

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.NhsVacancies.Should().BeEquivalentTo(new List<LiveVacancy>());
    }
    

    private string _nhsNoResultsResponse =
        "<nhsJobs>\n<totalPages>0</totalPages>\n<totalResults>0</totalResults>\n</nhsJobs>";

    private string _nhsResponse =
        "<nhsJobs>\n<vacancyDetails>\n<id>3394135</id>\n<reference>C9309-24-0264</reference>\n<title>Leadership and Management Trainee, Band 6</title>\n<description>Who is the scheme for? University College London Hospitals are seeking top class leaders of the future with the potential to make a difference at one ...</description>\n<employer>University College London Hospitals NHS Foundation Trust</employer>\n<type>Apprenticeship</type>\n<salary>\u00a337578.60 to \u00a339709.50</salary>\n<closeDate>2024-03-06</closeDate>\n<postDate>2024-02-12T15:57:11.165</postDate>\n<url>https://beta.jobs.nhs.uk/candidate/jobadvert/C9309-24-0264</url>\n<locations>\n<location>London, NW1 2PG</location>\n</locations>\n</vacancyDetails><vacancyDetails>\n<id>3446824</id>\n<reference>M9439-24-0020</reference>\n<title>Support Team Apprentice</title>\n<description>CDD Services provides procurement services and systems to a number of NHS organisations including County Durham and Darlington NHS Foundation Trust. C...</description>\n<employer>CDD Services (Synchronicity Care Ltd)</employer>\n<type>Apprenticeship</type>\n<salary>\u00a310295.00</salary>\n<closeDate>2024-03-20</closeDate>\n<postDate>2024-02-28T12:02:16.671</postDate>\n<url>https://beta.jobs.nhs.uk/candidate/jobadvert/M9439-24-0020</url>\n<locations>\n<location>Peterlee, SR8 2RU</location>\n</locations>\n</vacancyDetails>\n<totalPages>2</totalPages>\n<totalResults>54</totalResults>\n</nhsJobs>";
    private string _nhsResponse2 =
        "<nhsJobs>\n<vacancyDetails>\n<id>4394135</id>\n<reference>C9309-24-0264</reference>\n<title>Leadership and Management Trainee, Band 6</title>\n<description>Who is the scheme for? University College London Hospitals are seeking top class leaders of the future with the potential to make a difference at one ...</description>\n<employer>University College London Hospitals NHS Foundation Trust</employer>\n<type>Apprenticeship</type>\n<salary>\u00a337578.60 to \u00a339709.50</salary>\n<closeDate>2024-03-06</closeDate>\n<postDate>2024-02-12T15:57:11.165</postDate>\n<url>https://beta.jobs.nhs.uk/candidate/jobadvert/C9309-24-0264</url>\n<locations>\n<location>London, NW1 2PG</location>\n</locations>\n</vacancyDetails><vacancyDetails>\n<id>4446824</id>\n<reference>M9439-24-0020</reference>\n<title>Support Team Apprentice</title>\n<description>CDD Services provides procurement services and systems to a number of NHS organisations including County Durham and Darlington NHS Foundation Trust. C...</description>\n<employer>CDD Services (Synchronicity Care Ltd)</employer>\n<type>Apprenticeship</type>\n<salary>\u00a310295.00</salary>\n<closeDate>2024-03-20</closeDate>\n<postDate>2024-02-28T12:02:16.671</postDate>\n<url>https://beta.jobs.nhs.uk/candidate/jobadvert/M9439-24-0020</url>\n<locations>\n<location>Peterlee, SR8 2RU</location>\n</locations>\n</vacancyDetails>\n<totalPages>2</totalPages>\n<totalResults>54</totalResults>\n</nhsJobs>";
}