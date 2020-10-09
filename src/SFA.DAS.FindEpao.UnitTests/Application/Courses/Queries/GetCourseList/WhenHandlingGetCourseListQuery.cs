﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseList;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindEpao.UnitTests.Application.Courses.Queries.GetCourseList
{
    public class WhenHandlingGetCourseListQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Courses_Api(
            GetCourseListQuery query,
            GetStandardsListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetCourseListQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetStandardsRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
        }
    }
}