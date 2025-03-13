using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseByLarsCode;

public sealed class WhenGettingCourseByLarsCode
{
    private Mock<ICoursesApiClient<CoursesApiConfiguration>> _coursesApiClientMock;
    private Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> _roatpCourseManagementApiClientMock;
    private GetCourseByLarsCodeQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _coursesApiClientMock = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        _roatpCourseManagementApiClientMock = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();

        _handler = new GetCourseByLarsCodeQueryHandler(
            _coursesApiClientMock.Object,
            _roatpCourseManagementApiClientMock.Object
        );
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_Returns_Correct_CourseDetails(GetCourseByLarsCodeQuery query)
    {
        var standardDetailResponse = new StandardDetailResponse
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new ApprenticeshipFunding { MaxEmployerLevyCap = 5000, Duration = 12 },
                new ApprenticeshipFunding { MaxEmployerLevyCap = 6000, Duration = 18 }
            },
            Ksbs = new List<KsbResponse>
            {
                new KsbResponse { Type = GetCourseByLarsCodeQueryHandler.KsbsSkillsType, Description = "Skill A" },
                new KsbResponse { Type = GetCourseByLarsCodeQueryHandler.KsbsKnowledgeType, Description = "Knowledge A" },
                new KsbResponse { Type = GetCourseByLarsCodeQueryHandler.KsbsBehaviorType, Description = "Behaviour A" }
            }
        };

        var courseProvidersResponse = new GetCourseTrainingProvidersCountResponse
        {
            Courses = new List<CourseTrainingProviderCountModel>
            {
                new CourseTrainingProviderCountModel 
                { 
                    ProvidersCount = 10, 
                    TotalProvidersCount = 20 
                }
            }
        };

        _coursesApiClientMock
            .Setup(x => 
                x.GetWithResponseCode<StandardDetailResponse>(
                    It.Is<GetStandardDetailsByIdRequest>(a =>
                        a.Id.Equals(query.LarsCode.ToString())
                    )
                )
            )
            .ReturnsAsync(new ApiResponse<StandardDetailResponse>(standardDetailResponse, HttpStatusCode.OK, string.Empty));

        _roatpCourseManagementApiClientMock
            .Setup(x => 
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.IsAny<GetCourseTrainingProvidersCountRequest>())
                )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    courseProvidersResponse, 
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        var sut = await _handler.Handle(query, CancellationToken.None);

        Assert.Multiple(() => 
        { 
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.MaxFunding, Is.EqualTo(6000));
            Assert.That(sut.TypicalDuration, Is.EqualTo(18));
            Assert.That(sut.ProvidersCountWithinDistance, Is.EqualTo(10));
            Assert.That(sut.TotalProvidersCount, Is.EqualTo(20));
            Assert.That(sut.Skills[0], Is.EqualTo("Skill A"));
            Assert.That(sut.Knowledge[0], Is.EqualTo("Knowledge A"));
            Assert.That(sut.Behaviours[0], Is.EqualTo("Behaviour A"));
        });

        _coursesApiClientMock.Verify(x => 
            x.GetWithResponseCode<StandardDetailResponse>(
                It.Is<GetStandardDetailsByIdRequest>(r => 
                    r.Id == query.LarsCode.ToString()
                )
            ), Times.Once);

        _roatpCourseManagementApiClientMock.Verify(x => 
            x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                It.IsAny<GetCourseTrainingProvidersCountRequest>()
            ), Times.Once);
    }

    [Test]
    public async Task Handle_Returns_Default_When_No_Providers()
    {
        var query = new GetCourseByLarsCodeQuery { LarsCode = 456 };

        var standardDetailResponse = new StandardDetailResponse
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new ApprenticeshipFunding { MaxEmployerLevyCap = 7000, Duration = 24 }
            },
            Ksbs = new List<KsbResponse> 
            { 
                new KsbResponse 
                { 
                    Type = GetCourseByLarsCodeQueryHandler.KsbsSkillsType,
                    Description = "Skill B" 
                } 
            }
        };

        var emptyCourseProvidersResponse = new GetCourseTrainingProvidersCountResponse
        {
            Courses = new List<CourseTrainingProviderCountModel>()
        };

        _coursesApiClientMock
            .Setup(x => x.GetWithResponseCode<StandardDetailResponse>(
                It.Is<GetStandardDetailsByIdRequest>(a => 
                    a.Id.Equals(query.LarsCode.ToString())
                )
            ))
            .ReturnsAsync(
                new ApiResponse<StandardDetailResponse>(
                    standardDetailResponse, 
                    HttpStatusCode.OK, 
                    string.Empty
                )
            );

        _roatpCourseManagementApiClientMock
            .Setup(x => 
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.Is<GetCourseTrainingProvidersCountRequest>(a => 
                        a.LarsCodes.SequenceEqual(new int[1] { query.LarsCode }) &&
                        a.Distance.Equals(query.Distance) &&
                        a.Latitude.Equals(query.Lat) &&
                        a.Longitude.Equals(query.Lon)
                    )
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    emptyCourseProvidersResponse, 
                    HttpStatusCode.OK, 
                    string.Empty
                )
            );

        var sut = await _handler.Handle(query, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.MaxFunding, Is.EqualTo(7000));
            Assert.That(sut.TypicalDuration, Is.EqualTo(24));
            Assert.That(sut.ProvidersCountWithinDistance, Is.EqualTo(0));
            Assert.That(sut.TotalProvidersCount, Is.EqualTo(0));
        });
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_Returns_Correct_Skills(GetCourseByLarsCodeQuery query)
    {
        var standardDetailResponse = new StandardDetailResponse
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new ApprenticeshipFunding { MaxEmployerLevyCap = 7000, Duration = 24 }
            },
            Ksbs = new List<KsbResponse>
            {
                new KsbResponse
                {
                    Type = GetCourseByLarsCodeQueryHandler.KsbsSkillsType,
                    Description = "Skill A"
                }
            }
        };

        _coursesApiClientMock
            .Setup(x => x.GetWithResponseCode<StandardDetailResponse>(
                It.Is<GetStandardDetailsByIdRequest>(a =>
                    a.Id.Equals(query.LarsCode.ToString())
                )
            ))
            .ReturnsAsync(
                new ApiResponse<StandardDetailResponse>(
                    standardDetailResponse,
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        _roatpCourseManagementApiClientMock
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.IsAny<GetCourseTrainingProvidersCountRequest>()
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    new GetCourseTrainingProvidersCountResponse(),
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        var sut = await _handler.Handle(query, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(sut.Skills.Length, Is.EqualTo(1));
            Assert.That(sut.Skills[0], Is.EqualTo("Skill A"));
        });
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_Returns_Correct_Knowledge(GetCourseByLarsCodeQuery query)
    {
        var standardDetailResponse = new StandardDetailResponse
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new ApprenticeshipFunding { MaxEmployerLevyCap = 7000, Duration = 24 }
            },
            Ksbs = new List<KsbResponse>
            {
                new KsbResponse
                {
                    Type = GetCourseByLarsCodeQueryHandler.KsbsKnowledgeType,
                    Description = "Knowledge A"
                }
            }
        };

        _coursesApiClientMock
            .Setup(x => x.GetWithResponseCode<StandardDetailResponse>(
                It.Is<GetStandardDetailsByIdRequest>(a =>
                    a.Id.Equals(query.LarsCode.ToString())
                )
            ))
            .ReturnsAsync(
                new ApiResponse<StandardDetailResponse>(
                    standardDetailResponse,
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        _roatpCourseManagementApiClientMock
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.IsAny<GetCourseTrainingProvidersCountRequest>()
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    new GetCourseTrainingProvidersCountResponse(),
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        var sut = await _handler.Handle(query, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(sut.Knowledge.Length, Is.EqualTo(1));
            Assert.That(sut.Knowledge[0], Is.EqualTo("Knowledge A"));
        });
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_Returns_Correct_Behaviors(GetCourseByLarsCodeQuery query)
    {
        var standardDetailResponse = new StandardDetailResponse
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new ApprenticeshipFunding { MaxEmployerLevyCap = 7000, Duration = 24 }
            },
            Ksbs = new List<KsbResponse>
            {
                new KsbResponse
                {
                    Type = GetCourseByLarsCodeQueryHandler.KsbsBehaviorType,
                    Description = "Behavior A"
                }
            }
        };

        _coursesApiClientMock
            .Setup(x => x.GetWithResponseCode<StandardDetailResponse>(
                It.Is<GetStandardDetailsByIdRequest>(a =>
                    a.Id.Equals(query.LarsCode.ToString())
                )
            ))
            .ReturnsAsync(
                new ApiResponse<StandardDetailResponse>(
                    standardDetailResponse,
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        _roatpCourseManagementApiClientMock
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.IsAny<GetCourseTrainingProvidersCountRequest>()
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    new GetCourseTrainingProvidersCountResponse(),
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        var sut = await _handler.Handle(query, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(sut.Behaviours.Length, Is.EqualTo(1));
            Assert.That(sut.Behaviours[0], Is.EqualTo("Behavior A"));
        });
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_Returns_Default_Funding_When_TrainingProviderCount_Is_Null(GetCourseByLarsCodeQuery query)
    {
        _coursesApiClientMock
            .Setup(x => x.GetWithResponseCode<StandardDetailResponse>(
                It.Is<GetStandardDetailsByIdRequest>(a =>
                    a.Id.Equals(query.LarsCode.ToString())
                )
            ))
            .ReturnsAsync(
                new ApiResponse<StandardDetailResponse>(
                    new StandardDetailResponse() { Ksbs = [] },
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        _roatpCourseManagementApiClientMock
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.IsAny<GetCourseTrainingProvidersCountRequest>()
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    new GetCourseTrainingProvidersCountResponse(),
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        var sut = await _handler.Handle(query, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(sut.TypicalDuration, Is.EqualTo(0));
            Assert.That(sut.MaxFunding, Is.EqualTo(0));
        });
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_Returns_Default_Provider_Counts_When_TrainingProviderCount_Is_Null(GetCourseByLarsCodeQuery query)
    {
        _coursesApiClientMock
            .Setup(x => x.GetWithResponseCode<StandardDetailResponse>(
                It.Is<GetStandardDetailsByIdRequest>(a =>
                    a.Id.Equals(query.LarsCode.ToString())
                )
            ))
            .ReturnsAsync(
                new ApiResponse<StandardDetailResponse>(
                    new StandardDetailResponse() { Ksbs = [] },
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        _roatpCourseManagementApiClientMock
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.IsAny<GetCourseTrainingProvidersCountRequest>()
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    new GetCourseTrainingProvidersCountResponse(),
                    HttpStatusCode.OK,
                    string.Empty
                )
            );

        var sut = await _handler.Handle(query, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(sut.TotalProvidersCount, Is.EqualTo(0));
            Assert.That(sut.ProvidersCountWithinDistance, Is.EqualTo(0));
        });
    }
}
