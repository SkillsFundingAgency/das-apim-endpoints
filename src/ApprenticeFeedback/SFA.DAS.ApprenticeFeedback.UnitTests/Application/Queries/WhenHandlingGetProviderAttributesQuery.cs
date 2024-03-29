﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetAttributes;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Queries
{
    public class WhenHandlingGetProviderAttributesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Provider_Attributes_From_The_Api(
            List<Attribute> providerAttributesResponse,
            GetAttributesQuery query,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockProviderAttributesApiClient,
            GetAttributesQueryHandler handler)
        {
            mockProviderAttributesApiClient
                .Setup(client => client.Get<List<Attribute>>(
                It.IsAny<GetAttributesRequest>()))
                .ReturnsAsync(providerAttributesResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Attributes.Should().BeEquivalentTo(providerAttributesResponse);
        }
    }
}
