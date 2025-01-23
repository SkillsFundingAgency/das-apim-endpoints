using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Applications
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task AndNoApplicationsExistReturnsEmptyList(GetApplicationsQuery query,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<ILevyTransferMatchingService> levyTransferMatchingService,
            GetApplicationsQueryHandler handler)
        {
            levyTransferMatchingService.Setup(o => o.GetApplications(It.Is<GetApplicationsRequest>(o => o.AccountId == query.AccountId))).ReturnsAsync(
                new GetApplicationsResponse
                {
                    Applications = new List<GetApplicationsResponse.Application>()
                }
            );

            var result = await handler.Handle(query, CancellationToken.None);
            result.Items.Count.Should().Be(0);
            coursesApiClient.Verify(o => o.Get<GetStandardsListItem>(It.IsAny<GetStandardDetailsByIdRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task AndApplicationsExistReturnsListOfApplications(GetApplicationsQuery query,
            GetApplicationsResponse response,
            int pageSize,
            [Frozen] Mock<ILevyTransferMatchingService> levyTransferMatchingService,
            GetApplicationsQueryHandler handler)
        {
            query.PageSize = pageSize;

            //response.Applications = new List<GetApplicationsResponse.Application>()
            //{
            //    response.Applications.First()
            //};
            
            response.Page = query.Page;
            response.PageSize = query.PageSize.Value;
            response.TotalItems = response.Applications.Count();
            response.TotalPages = (int)Math.Ceiling((double)response.TotalItems / response.PageSize);

            levyTransferMatchingService.Setup(o => o.GetApplications(It.Is<GetApplicationsRequest>(o => 
                    o.AccountId == query.AccountId
                    && o.Page == query.Page
                    && o.PageSize == query.PageSize
                ))).ReturnsAsync(
                   response
                );

            var result = await handler.Handle(query, CancellationToken.None);

            result.Items.Count.Should().BeGreaterThan(0);
            result.Items.Count.Should().Be(response.Applications.Count());

            result.TotalItems.Should().Be(response.TotalItems);
            result.TotalPages.Should().Be(response.TotalPages);
            result.Page.Should().Be(response.Page);
            result.PageSize.Should().Be(response.PageSize);

            var expectedApplication = response.Applications.First();
            var actualApplication = result.Items.First();
            actualApplication.Id.Should().Be(expectedApplication.Id);
            actualApplication.DasAccountName.Should().Be(expectedApplication.DasAccountName);
            actualApplication.PledgeId.Should().Be(expectedApplication.PledgeId);
            actualApplication.NumberOfApprentices.Should().Be(expectedApplication.NumberOfApprentices);
            actualApplication.Amount.Should().Be(expectedApplication.TotalAmount);
            actualApplication.TotalAmount.Should().Be(expectedApplication.TotalAmount);
            actualApplication.CreatedOn.Should().Be(expectedApplication.CreatedOn);
            actualApplication.IsNamePublic.Should().Be(expectedApplication.IsNamePublic);
            actualApplication.Status.Should().Be(expectedApplication.Status);
        }
    }
}
