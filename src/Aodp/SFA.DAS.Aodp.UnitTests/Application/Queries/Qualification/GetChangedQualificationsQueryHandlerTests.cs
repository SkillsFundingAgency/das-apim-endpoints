using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Azure.Amqp.Framing;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualification;

[TestFixture]
public class GetChangedQualificationsQueryHandlerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IAodpApiClient<AodpApiConfiguration>> _apiClient = new();
    public GetChangedQualificationsQueryHandler _getChangedQualificationsQueryHandler { get; set; }

    public GetChangedQualificationsQueryHandlerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _getChangedQualificationsQueryHandler = new(_apiClient.Object);
    }

    [Test]
    public async Task Test_Get_Changed_Qualification()
    {
        var qualifications = _fixture.CreateMany<GetChangedQualificationsQueryResponse.ChangedQualification>().ToList();
        var responseType = new BaseMediatrResponse<GetChangedQualificationsQueryResponse>();
        responseType.Value.Data = qualifications;
        _apiClient.Setup(v => v.Get<BaseMediatrResponse<GetChangedQualificationsQueryResponse>>(It.IsAny<GetChangedQualificationsApiRequest>()))
            .Returns(Task.FromResult(responseType));

        var response = await _getChangedQualificationsQueryHandler.Handle(new GetChangedQualificationsQuery(), default);

        Assert.That(response.Success, Is.True);
    }

    [Test]
    public async Task Test_Get_Changed_Qualification_Throws_Returns_Error()
    {
        _apiClient.Setup(v => v.Get<BaseMediatrResponse<GetChangedQualificationsQueryResponse>>(It.IsAny<GetChangedQualificationsApiRequest>()))
            .Throws(new Exception());

        var response = await _getChangedQualificationsQueryHandler.Handle(new GetChangedQualificationsQuery(), default);

        Assert.That(response.Success, Is.False);
    }
}
