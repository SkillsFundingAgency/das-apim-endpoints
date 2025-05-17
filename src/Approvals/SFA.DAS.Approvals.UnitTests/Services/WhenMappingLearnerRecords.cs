﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Services;

public class WhenMappingLearnerRecords
{
    [Test, MoqAutoData]
    public async Task Then_It_Maps_The_Main_Fields_Correctly(
        LearnerDataRecord inputDataRecord,
        List<GetStandardsListItem> standards,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApi,
        [Greedy] MapLearnerRecords sut
    )
    {
        var result = await sut.Map(new List<LearnerDataRecord> {inputDataRecord}, standards);

        result[0].Id.Should().Be(inputDataRecord.Id);
        result[0].FirstName.Should().Be(inputDataRecord.FirstName);
        result[0].LastName.Should().Be(inputDataRecord.LastName);
        result[0].Uln.Should().Be(inputDataRecord.Uln);
    }

    [Test, MoqAutoData]
    public async Task Then_It_Maps_The_Course_Field_Correctly(
        LearnerDataRecord[] inputDataRecords,
        List<GetStandardsListItem> courses,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApi,
        [Greedy] MapLearnerRecords sut
    )
    {
        courses[0].LarsCode = inputDataRecords[0].StandardCode;
        courses[1].LarsCode = inputDataRecords[1].StandardCode;

        var result = await sut.Map(inputDataRecords, courses);

        result[0].Course.Should().Be(courses[0].Title);
        result[1].Course.Should().Be(courses[1].Title);
    }
}