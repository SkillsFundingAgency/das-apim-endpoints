using System.Text.Json;
using SFA.DAS.Aodp.Application.Queries.Qualifications;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualifications
{
    [TestFixture]
    public class GetQualificationsExportResponseTests
    {
        [Test]
        public void Deserialise_InnerApiJson_PopulatesFundingFields()
        {
            var json = "{\"qualificationExports\":[{" +
                       "\"qualificationNumber\":\"99999991\"," +
                       "\"intentionToSeekFundingInEngland\":true," +
                       "\"fundingInWales\":null," +
                       "\"fundingInNorthernIreland\":null}]}";

            var result = JsonSerializer.Deserialize<GetQualificationsExportResponse>(
                json, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var export = result!.QualificationExports.Single();

            Assert.Multiple(() =>
            {
                Assert.That(export.QualificationNumber, Is.EqualTo("99999991"));
                Assert.That(export.IntentionToSeekFundingInEngland, Is.True);
                Assert.That(export.FundingInWales, Is.Null);
                Assert.That(export.FundingInNorthernIreland, Is.Null);
            });
        }
    }
}