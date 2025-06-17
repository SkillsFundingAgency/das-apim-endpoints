using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Services;

public class WhenAddingExtendedPropertiesToCsvRecords
{
    [Test, MoqAutoData]
    public async Task Then_It_Add_Returns_Each_Record_Mapped_ToExtendedObject(
        List<BulkUploadAddDraftApprenticeshipRequest> csvRecords,
        BulkUploadAddDraftApprenticeshipExtendedRequest mappedRecord,
        [Frozen] Mock<ICourseTypesApiClient> coursesApi,
        [Frozen] Mock<IMapper> mapper,
        [Greedy] AddCourseTypeDataToCsvService sut
    )
    {
        csvRecords.ForEach(x => x.CourseCode = null);
        mappedRecord.CourseCode = null;
        mapper.Setup(x => x.Map<BulkUploadAddDraftApprenticeshipExtendedRequest>(It.IsAny<BulkUploadAddDraftApprenticeshipRequest>())).
            Returns(mappedRecord);

        var result = await sut.PopulateWithCourseTypeData(csvRecords);
        result.FirstOrDefault().Should().BeEquivalentTo(mappedRecord);
    }

    [Test, MoqAutoData]
    public async Task Then_It_Add_Age_Range_Limits_To_Each_Record(
        BulkUploadAddDraftApprenticeshipRequest csvRecord,
        BulkUploadAddDraftApprenticeshipExtendedRequest mappedRecord,
        GetLearnerAgeResponse apiResponse,
        [Frozen] Mock<ICourseTypesApiClient> coursesApi,
        [Frozen] Mock<IMapper> mapper,
        [Greedy] AddCourseTypeDataToCsvService sut
    )
    {
        coursesApi.Setup(x => x.GetWithResponseCode<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()))
            .ReturnsAsync(new ApiResponse<GetLearnerAgeResponse>(apiResponse, HttpStatusCode.OK, null));

        mapper.Setup(x => x.Map<BulkUploadAddDraftApprenticeshipExtendedRequest>(It.IsAny<BulkUploadAddDraftApprenticeshipRequest>())).
            Returns(mappedRecord);

        var result = await sut.PopulateWithCourseTypeData(new List<BulkUploadAddDraftApprenticeshipRequest> { csvRecord });
        result.FirstOrDefault().Should().BeEquivalentTo(mappedRecord);
        result[0].MinimumAgeAtApprenticeshipStart.Should().Be(apiResponse.MinimumAge);
        result[0].MaximumAgeAtApprenticeshipStart.Should().Be(apiResponse.MaximumAge);
    }


    [Test, MoqAutoData]
    public async Task When_On_Same_Course_Then_It_Only_Calls_Api_Once(
        List<BulkUploadAddDraftApprenticeshipRequest> csvRecords,
        BulkUploadAddDraftApprenticeshipExtendedRequest mappedRecord,
        GetLearnerAgeResponse apiResponse,
        [Frozen] Mock<ICourseTypesApiClient> coursesApi,
        [Frozen] Mock<IMapper> mapper,
        [Greedy] AddCourseTypeDataToCsvService sut
    )
    {
        csvRecords.ForEach(x => x.CourseCode = "ABCDE");
        mappedRecord.CourseCode = null;

        coursesApi.Setup(x => x.GetWithResponseCode<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()))
            .ReturnsAsync(new ApiResponse<GetLearnerAgeResponse>(apiResponse, HttpStatusCode.OK, null));

        mapper.Setup(x => x.Map<BulkUploadAddDraftApprenticeshipExtendedRequest>(It.IsAny<BulkUploadAddDraftApprenticeshipRequest>())).
            Returns(mappedRecord);

        var result = await sut.PopulateWithCourseTypeData(csvRecords);
        coursesApi.Verify(x=>x.GetWithResponseCode<GetLearnerAgeResponse>(It.IsAny<GetLearnerAgeRequest>()), Times.Once);
    }
}