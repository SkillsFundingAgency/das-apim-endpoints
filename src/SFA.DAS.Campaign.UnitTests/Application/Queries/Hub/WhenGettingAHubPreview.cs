﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Queries.PreviewHub;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Hub
{
    public class WhenGettingAHubPreview
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Valid_Request_Parameters_And_The_Hub_Is_Returned(
            GetPreviewHubQuery query,
            CmsContent apiResponse,
            HubPageModel response,
            [Frozen] Mock<IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration>> apiClient,
            GetPreviewHubQueryHandler handler)
        {
            apiClient.Setup(o =>
                    o.Get<CmsContent>(
                        It.Is<GetHubEntriesRequest>(c =>
                            c.GetUrl.Contains($"fields.hubType={query.Hub.ToTitleCase()}"))))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.PageModel.Should().BeEquivalentTo(response.Build(apiResponse));
        }
    }
}
