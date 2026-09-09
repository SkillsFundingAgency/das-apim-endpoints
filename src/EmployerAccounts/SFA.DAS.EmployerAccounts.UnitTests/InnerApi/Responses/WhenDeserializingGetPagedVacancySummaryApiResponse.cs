using System;
using System.Text.Json;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;

namespace SFA.DAS.EmployerAccounts.UnitTests.InnerApi.Responses;

public class WhenDeserializingGetPagedVacancySummaryApiResponse
{
    [Test]
    public void Then_Archived_Status_Deserializes_Successfully()
    {
        const string json = """
            {
              "pageInfo": {
                "totalCount": 1,
                "pageIndex": 1,
                "pageSize": 1,
                "totalPages": 1,
                "hasPreviousPage": false,
                "hasNextPage": false
              },
              "items": [
                {
                  "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
                  "title": "Archived vacancy",
                  "status": "Archived",
                  "noOfNewApplications": 0,
                  "noOfSuccessfulApplications": 0,
                  "noOfUnsuccessfulApplications": 0,
                  "closingDate": null,
                  "closedDate": null
                }
              ]
            }
            """;

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var actual = JsonSerializer.Deserialize<GetPagedVacancySummaryApiResponse>(json, options);

        actual.Should().NotBeNull();
        actual!.PageInfo.TotalCount.Should().Be(1);
        actual.Items.Should().HaveCount(1);
        actual.Items[0].Status.Should().Be(VacancyStatus.Archived);
        actual.Items[0].Title.Should().Be("Archived vacancy");
        actual.Items[0].Id.Should().Be(Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"));
    }
}
