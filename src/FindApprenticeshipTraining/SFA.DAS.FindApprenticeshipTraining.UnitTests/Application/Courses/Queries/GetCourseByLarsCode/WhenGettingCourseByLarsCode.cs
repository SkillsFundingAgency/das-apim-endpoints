using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
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
    private Mock<ICachedLocationLookupService> _cachedLocationLookupService;
    private GetCourseByLarsCodeQueryHandler _handler;


    [SetUp]
    public void Setup()
    {
        _coursesApiClientMock = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
        _roatpCourseManagementApiClientMock = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
        _cachedLocationLookupService = new Mock<ICachedLocationLookupService>();

        _handler = new GetCourseByLarsCodeQueryHandler(
            _coursesApiClientMock.Object,
            _roatpCourseManagementApiClientMock.Object,
            _cachedLocationLookupService.Object
        );
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_Returns_Correct_CourseDetails(GetCourseByLarsCodeQuery query)
    {
        string apprenticeshipType = "FoundationApprenticeship";

        var standardDetailResponse = new StandardDetailResponse
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new ApprenticeshipFunding { MaxEmployerLevyCap = 5000, Duration = 12 },
                new ApprenticeshipFunding { MaxEmployerLevyCap = 6000, Duration = 18 }
            },
            ApprenticeshipType = apprenticeshipType
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
            Assert.That(sut.ApprenticeshipType, Is.EqualTo(apprenticeshipType));
            Assert.That(sut.ProvidersCountWithinDistance, Is.EqualTo(10));
            Assert.That(sut.TotalProvidersCount, Is.EqualTo(20));
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
                        a.Distance.Equals(query.Distance)
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
    public async Task Handler_Returns_Ksbs(GetCourseByLarsCodeQuery query)
    {
        var ksbType = "Knowledge";
        var ksbId = Guid.NewGuid();
        var ksbDescription = "Skill A";
        var standardDetailResponse = new StandardDetailResponse
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new() { MaxEmployerLevyCap = 7000, Duration = 24 }
            },
            Ksbs = new List<KsbResponse>
            {
                new()
                {
                    Type = ksbType,
                    Id = ksbId,
                    Description = ksbDescription
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
            Assert.That(sut.Ksbs.Count, Is.EqualTo(1));
            Assert.That(sut.Ksbs[0].Detail, Is.EqualTo(ksbDescription));
            Assert.That(sut.Ksbs[0].Id, Is.EqualTo(ksbId));
            Assert.That(sut.Ksbs[0].Type, Is.EqualTo(ksbType));
        });
    }


    [Test]
    [MoqAutoData]
    public async Task Handler_Returns_RelatedOccupations(GetCourseByLarsCodeQuery query)
    {
        var relatedOccupationsTitle1 = "Plumbing and heating technician";
        var relatedOccupationsLevel1 = 2;
        var relatedOccupationsTitle2 = "Refrigeration technician";
        var relatedOccupationsLevel2 = 2;

        var standardDetailResponse = new StandardDetailResponse
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new() { MaxEmployerLevyCap = 7000, Duration = 24 }
            },
            RelatedOccupations = new List<RelatedOccupation>
           {
               new(relatedOccupationsTitle1, relatedOccupationsLevel1),
               new(relatedOccupationsTitle2,relatedOccupationsLevel2)
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
            Assert.That(sut.RelatedOccupations.Count, Is.EqualTo(2));
            Assert.That(sut.RelatedOccupations[0].Title, Is.EqualTo(relatedOccupationsTitle1));
            Assert.That(sut.RelatedOccupations[1].Title, Is.EqualTo(relatedOccupationsTitle2));
            Assert.That(sut.RelatedOccupations[0].Level, Is.EqualTo(relatedOccupationsLevel1));
            Assert.That(sut.RelatedOccupations[1].Level, Is.EqualTo(relatedOccupationsLevel2));
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

    [Test]
    [MoqAutoData]
    public async Task Handle_Gets_Location_Information_And_Queries_Provider_Count_By_Longitude_And_Latitude(
        LocationItem locationItem,
        StandardDetailResponse standardDetailsResponse,
        GetCourseTrainingProvidersCountResponse courseTrainingProvidersCountResponse
    )
    {
        var query = new GetCourseByLarsCodeQuery { LarsCode = 456, Location = "sw1", Distance = 10 };

        _cachedLocationLookupService.Setup(x =>
            x.GetCachedLocationInformation(query.Location, false)
        ).ReturnsAsync(locationItem);

        _coursesApiClientMock
            .Setup(x =>
                x.GetWithResponseCode<StandardDetailResponse>(
                    It.Is<GetStandardDetailsByIdRequest>(a =>
                        a.Id.Equals(query.LarsCode.ToString())
                    )
                )
            )
            .ReturnsAsync(new ApiResponse<StandardDetailResponse>(standardDetailsResponse, HttpStatusCode.OK, string.Empty));

        _roatpCourseManagementApiClientMock
                .Setup(x =>
                    x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                            It.Is<GetCourseTrainingProvidersCountRequest>(a =>
                                a.LarsCodes.SequenceEqual(new int[1] { query.LarsCode }) &&
                                a.Distance.Equals(query.Distance) &&
                                a.Latitude.Equals((decimal?)locationItem.GeoPoint[0]) &&
                                a.Longitude.Equals((decimal?)locationItem.GeoPoint[1])
                            )
                        )
                    )
                .ReturnsAsync(
                    new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                        courseTrainingProvidersCountResponse,
                        HttpStatusCode.OK,
                        string.Empty
                    )
                );

        var sut = await _handler.Handle(query, CancellationToken.None);

        Assert.That(sut, Is.Not.Null);

        _cachedLocationLookupService.Verify(x =>
            x.GetCachedLocationInformation(query.Location, false
        ), Times.Once);
    }


    [TestCase(-1, 1, 1, null, 6)]
    [TestCase(-1, 1, 1, 2, 6)]
    [TestCase(1, null, -1, 1, 15)]
    [TestCase(1, 2, -1, 1, 15)]
    [TestCase(1, 2, 2, 5, 0)]
    [TestCase(-10, -5, -20, -15, 0)]
    public async Task Handle_Returns_Correct_IncentivePayment(int firstStartDateIncrement, int? firstEndDateIncrement,
                                                                 int secondStartDateIncrement, int? secondEndDateIncrement,
                                                                int expectedIncentivePayment)
    {
        int larsCode = 111;
        int firstPayment1 = 1;
        int firstPayment2 = 2;
        int firstPayment3 = 3;
        int secondPayment1 = 4;
        int secondPayment2 = 5;
        int secondPayment3 = 6;

        DateTime? firstEffectiveTo = null;
        if (firstEndDateIncrement != null) firstEffectiveTo = DateTime.Today.AddDays(firstEndDateIncrement.Value);
        DateTime? secondEffectiveTo = null;
        if (secondEndDateIncrement != null) secondEffectiveTo = DateTime.Today.AddDays(secondEndDateIncrement.Value);


        GetCourseByLarsCodeQuery query = new GetCourseByLarsCodeQuery { LarsCode = larsCode };

        var standardDetailResponse = new StandardDetailResponse
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new ApprenticeshipFunding { MaxEmployerLevyCap = 5000, Duration = 12,
                    EffectiveFrom = DateTime.Today.AddDays(firstStartDateIncrement),
                    EffectiveTo = firstEffectiveTo,
                    FoundationAppFirstEmpPayment = firstPayment1,
                    FoundationAppSecondEmpPayment = firstPayment2,
                    FoundationAppThirdEmpPayment = firstPayment3,
                },
                new ApprenticeshipFunding { MaxEmployerLevyCap = 5000, Duration = 12,
                    EffectiveFrom = DateTime.Today.AddDays(secondStartDateIncrement),
                    EffectiveTo = secondEffectiveTo,
                    FoundationAppFirstEmpPayment = secondPayment1,
                    FoundationAppSecondEmpPayment = secondPayment2,
                    FoundationAppThirdEmpPayment = secondPayment3,
                },
            }
        };

        var courseProvidersResponse = new GetCourseTrainingProvidersCountResponse
        {
            Courses = new List<CourseTrainingProviderCountModel>
            {
                new()
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
            Assert.That(sut.IncentivePayment, Is.EqualTo(expectedIncentivePayment));
        });

    }


    [Test]
    public async Task Handle_Returns_Zero_IncentivePayment_When_No_Apprenticeship_Funding()
    {
        var larsCode = 123;

        GetCourseByLarsCodeQuery query = new GetCourseByLarsCodeQuery { LarsCode = larsCode };

        var standardDetailResponse = new StandardDetailResponse
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding> { }
        };

        var courseProvidersResponse = new GetCourseTrainingProvidersCountResponse
        {
            Courses = new List<CourseTrainingProviderCountModel>
            {
                new()
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
            Assert.That(sut.IncentivePayment, Is.EqualTo(0));
        });
    }

    [TestCase(null, null, null, 0)]
    [TestCase(null, 2, 3, 5)]
    [TestCase(1, null, 3, 4)]
    [TestCase(1, 2, null, 3)]
    [TestCase(null, null, 3, 3)]
    [TestCase(null, 2, null, 2)]
    [TestCase(1, null, null, 1)]
    [TestCase(1, 2, 3, 6)]

    public async Task Handle_Returns_Expected_IncentivePayment_When_3_Payments_Set_Up(int? firstPayment, int? secondPayment, int? thirdPayment, int expectedIncentivePayment)
    {
        var larsCode = 123;

        GetCourseByLarsCodeQuery query = new GetCourseByLarsCodeQuery { LarsCode = larsCode };

        var standardDetailResponse = new StandardDetailResponse
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new()
                {
                    FoundationAppFirstEmpPayment = firstPayment,
                    FoundationAppSecondEmpPayment = secondPayment,
                    FoundationAppThirdEmpPayment = thirdPayment,
                    EffectiveFrom = DateTime.Today
                }
            }
        };

        var courseProvidersResponse = new GetCourseTrainingProvidersCountResponse
        {
            Courses = new List<CourseTrainingProviderCountModel>
            {
                new()
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
            Assert.That(sut.IncentivePayment, Is.EqualTo(expectedIncentivePayment));
        });
    }
}
