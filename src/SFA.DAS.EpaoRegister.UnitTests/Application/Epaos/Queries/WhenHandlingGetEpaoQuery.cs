using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpao;
using SFA.DAS.EpaoRegister.InnerApi.Requests;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EpaoRegister.UnitTests.Application.Epaos.Queries
{
    public class WhenHandlingGetEpaoQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Epao_From_Assessor_Api(
            GetEpaoQuery query,
            SearchEpaosListItem apiResponse,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetEpaoQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.GetAll<SearchEpaosListItem>(
                    It.Is<GetEpaoRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(new List<SearchEpaosListItem>{apiResponse});

            var result = await handler.Handle(query, CancellationToken.None);

            result.Epao.Should().BeEquivalentTo(apiResponse);
        }

        [Test, MoqAutoData]
        public void And_Multiple_Results_Then_Throws_ArgumentException(
            GetEpaoQuery query,
            List<SearchEpaosListItem> apiResponse,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetEpaoQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.GetAll<SearchEpaosListItem>(
                    It.Is<GetEpaoRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(apiResponse);

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            act.Should().Throw<ArgumentException>();
        }

        //todo: validation check (incomplete epao id)
    }
}