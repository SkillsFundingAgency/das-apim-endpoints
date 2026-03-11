using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseByLarsCode;

public sealed class WhenGettingCourseByLarsCode
{
    private Mock<ICachedStandardDetailsService> _cachedStandardDetailsService;
    private Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> _roatpCourseManagementApiClientMock;
    private Mock<ICachedLocationLookupService> _cachedLocationLookupService;
    private GetCourseByLarsCodeQueryHandler _handler;


    [SetUp]
    public void Setup()
    {
        _cachedStandardDetailsService = new Mock<ICachedStandardDetailsService>();
        _roatpCourseManagementApiClientMock = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
        _cachedLocationLookupService = new Mock<ICachedLocationLookupService>();

        _handler = new GetCourseByLarsCodeQueryHandler(
            _cachedStandardDetailsService.Object,
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

        _cachedStandardDetailsService
            .Setup(x =>
                x.GetStandardDetails(
                    It.Is<string>(a =>
                        a.Equals(query.LarsCode.ToString())
                    )
                )
            )
            .ReturnsAsync(standardDetailResponse);

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
            Assert.That(sut.ApprenticeshipType, Is.EqualTo(apprenticeshipType));
            Assert.That(sut.ProvidersCountWithinDistance, Is.EqualTo(10));
            Assert.That(sut.TotalProvidersCount, Is.EqualTo(20));
        });

        _cachedStandardDetailsService.Verify(x =>
            x.GetStandardDetails(
                It.Is<string>(r =>
                    r == query.LarsCode.ToString()
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
        var query = new GetCourseByLarsCodeQuery { LarsCode = "456" };

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

        _cachedStandardDetailsService
            .Setup(x =>
                x.GetStandardDetails(
                    It.Is<string>(a =>
                        a.Equals(query.LarsCode.ToString())
                    )
                )
            )
            .ReturnsAsync(standardDetailResponse);

        _roatpCourseManagementApiClientMock
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.Is<GetCourseTrainingProvidersCountRequest>(a =>
                        a.LarsCodes.SequenceEqual(new string[1] { query.LarsCode }) &&
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

        _cachedStandardDetailsService
            .Setup(x => x.GetStandardDetails(
                It.Is<string>(r =>
                    r == query.LarsCode.ToString()
                )
            ))
            .ReturnsAsync(standardDetailResponse);

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

        _cachedStandardDetailsService
            .Setup(x => x.GetStandardDetails(
                It.Is<string>(r =>
                    r == query.LarsCode.ToString()
                )
            ))
            .ReturnsAsync(standardDetailResponse);

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
        _cachedStandardDetailsService
            .Setup(x => x.GetStandardDetails(
                It.Is<string>(r =>
                    r == query.LarsCode.ToString()
                )
            ))
            .ReturnsAsync(new StandardDetailResponse() { Ksbs = [] }
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
        _cachedStandardDetailsService
             .Setup(x => x.GetStandardDetails(
                 It.Is<string>(r =>
                     r == query.LarsCode.ToString()
                 )
             ))
             .ReturnsAsync(new StandardDetailResponse() { Ksbs = [] }
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
        var query = new GetCourseByLarsCodeQuery { LarsCode = "456", Location = "sw1", Distance = 10 };

        _cachedLocationLookupService.Setup(x =>
            x.GetCachedLocationInformation(query.Location, false)
        ).ReturnsAsync(locationItem);

        _cachedStandardDetailsService
            .Setup(x =>
                x.GetStandardDetails(
                    It.Is<string>(r =>
                        r == query.LarsCode.ToString())
                )
            )
            .ReturnsAsync(standardDetailsResponse);
        _roatpCourseManagementApiClientMock
                .Setup(x =>
                    x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                            It.Is<GetCourseTrainingProvidersCountRequest>(a =>
                                a.LarsCodes.SequenceEqual(new string[1] { query.LarsCode }) &&
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

    [Test, AutoData]
    public async Task Handle_Gets_Funding_Data_From_Latest_Record(GetCourseByLarsCodeQuery query, StandardDetailResponse standardDetailResponse, GetCourseTrainingProvidersCountResponse courseProvidersResponse)
    {
        ApprenticeshipFunding futureFundingRecord = new()
        {
            MaxEmployerLevyCap = 10000,
            Duration = 18,
            FoundationAppFirstEmpPayment = 1000,
            FoundationAppSecondEmpPayment = 2000,
            FoundationAppThirdEmpPayment = 3000,
            EffectiveFrom = DateTime.Today.AddMonths(1)
        };

        ApprenticeshipFunding activeFundingRecord = new()
        {
            MaxEmployerLevyCap = 5000,
            Duration = 17,
            FoundationAppFirstEmpPayment = 100,
            FoundationAppSecondEmpPayment = 200,
            FoundationAppThirdEmpPayment = 300,
            EffectiveFrom = DateTime.Today,
            EffectiveTo = futureFundingRecord.EffectiveFrom.AddDays(-1)
        };
        ApprenticeshipFunding oldFundingRecord = new()
        {
            MaxEmployerLevyCap = 4000,
            Duration = 16,
            FoundationAppFirstEmpPayment = 10,
            FoundationAppSecondEmpPayment = 20,
            FoundationAppThirdEmpPayment = 30,
            EffectiveFrom = DateTime.Today.AddMonths(-10),
            EffectiveTo = activeFundingRecord.EffectiveFrom.AddDays(-1)
        };
        standardDetailResponse.ApprenticeshipFunding = [activeFundingRecord, futureFundingRecord, oldFundingRecord];

        _cachedStandardDetailsService
            .Setup(x => x.GetStandardDetails(
                It.Is<string>(r =>
                    r == query.LarsCode.ToString()
                )
            ))
            .ReturnsAsync(standardDetailResponse);
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
            Assert.That(sut.IncentivePayment, Is.EqualTo(6000));
            Assert.That(sut.MaxFunding, Is.EqualTo(10000));
            Assert.That(sut.TypicalDuration, Is.EqualTo(18));
        });
    }

    [Test, AutoData]
    public async Task Handle_Calculates_IncentivePayment_Using_The_Current_Active_Funding_Record_With_No_End_Date(GetCourseByLarsCodeQuery query, StandardDetailResponse standardDetailResponse, GetCourseTrainingProvidersCountResponse courseProvidersResponse)
    {
        ApprenticeshipFunding activeFundingRecordWithNoEndDate = new()
        {
            FoundationAppFirstEmpPayment = 1000,
            FoundationAppSecondEmpPayment = 2000,
            FoundationAppThirdEmpPayment = 3000,
            EffectiveFrom = DateTime.Today
        };
        ApprenticeshipFunding oldFundingRecord = new()
        {
            FoundationAppFirstEmpPayment = 10,
            FoundationAppSecondEmpPayment = 20,
            FoundationAppThirdEmpPayment = 30,
            EffectiveFrom = DateTime.Today.AddMonths(-10),
            EffectiveTo = activeFundingRecordWithNoEndDate.EffectiveFrom.AddDays(-1)
        };
        standardDetailResponse.ApprenticeshipFunding = [activeFundingRecordWithNoEndDate, oldFundingRecord];

        _cachedStandardDetailsService
                .Setup(x => x.GetStandardDetails(
                    It.Is<string>(r =>
                        r == query.LarsCode.ToString()
                    )
                ))
                .ReturnsAsync(standardDetailResponse);

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
            Assert.That(sut.IncentivePayment, Is.EqualTo(6000));
        });
    }

    [Test, AutoData]
    public async Task Handle_Calculates_IncentivePayment_Using_The_Current_Active_Funding_Record_With_End_Date_In_Future(GetCourseByLarsCodeQuery query, StandardDetailResponse standardDetailResponse, GetCourseTrainingProvidersCountResponse courseProvidersResponse)
    {
        ApprenticeshipFunding activeFundingRecordWithNoEndDate = new()
        {
            FoundationAppFirstEmpPayment = 1000,
            FoundationAppSecondEmpPayment = 2000,
            FoundationAppThirdEmpPayment = 3000,
            EffectiveFrom = DateTime.Today,
            EffectiveTo = DateTime.Today.AddDays(10)
        };
        ApprenticeshipFunding oldFundingRecord = new()
        {
            FoundationAppFirstEmpPayment = 10,
            FoundationAppSecondEmpPayment = 20,
            FoundationAppThirdEmpPayment = 30,
            EffectiveFrom = DateTime.Today.AddMonths(-10),
            EffectiveTo = DateTime.Today.AddDays(-1)
        };
        ApprenticeshipFunding futureFundingRecord = new()
        {
            FoundationAppFirstEmpPayment = 1,
            FoundationAppSecondEmpPayment = 2,
            FoundationAppThirdEmpPayment = 3,
            EffectiveFrom = oldFundingRecord.EffectiveTo.GetValueOrDefault().AddDays(1)
        };
        standardDetailResponse.ApprenticeshipFunding = [oldFundingRecord, activeFundingRecordWithNoEndDate, futureFundingRecord];

        _cachedStandardDetailsService
                .Setup(x => x.GetStandardDetails(
                    It.Is<string>(r =>
                        r == query.LarsCode.ToString()
                    )
                ))
                .ReturnsAsync(standardDetailResponse);

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
            Assert.That(sut.IncentivePayment, Is.EqualTo(6000));
        });
    }

    [Test]
    public async Task Handle_Returns_Zero_IncentivePayment_When_No_Apprenticeship_Funding()
    {
        var larsCode = "123";

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

        _cachedStandardDetailsService
                .Setup(x => x.GetStandardDetails(
                    It.Is<string>(r =>
                        r == query.LarsCode.ToString()
                    )
                ))
                .ReturnsAsync(standardDetailResponse);

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
        var larsCode = "123";

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
        _cachedStandardDetailsService
                .Setup(x => x.GetStandardDetails(
                    It.Is<string>(r =>
                        r == query.LarsCode.ToString()
                    )
                ))
                .ReturnsAsync(standardDetailResponse);

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

    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.BadRequest)]
    public async Task Handle_Returns_Null_When_Training_Providers_Returns_404_Or_400(HttpStatusCode statusCode)
    {
        _cachedStandardDetailsService
            .Setup(x => x.GetStandardDetails(It.IsAny<string>()))
            .ReturnsAsync(new StandardDetailResponse());

        _roatpCourseManagementApiClientMock
            .Setup(x =>
                x.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                    It.IsAny<GetCourseTrainingProvidersCountRequest>()
                )
            )
            .ReturnsAsync(
                new ApiResponse<GetCourseTrainingProvidersCountResponse>(
                    null,
                    statusCode,
                    string.Empty
                )
            );

        var sut = await _handler.Handle(new GetCourseByLarsCodeQuery(), CancellationToken.None);

        Assert.That(sut, Is.Null);
    }
}
