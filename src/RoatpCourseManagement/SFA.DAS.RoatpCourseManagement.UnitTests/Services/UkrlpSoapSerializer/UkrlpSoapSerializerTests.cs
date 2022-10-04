using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Services.UkrlpSoapSerializer
{
    [TestFixture]
    public class UkrlpSoapSerializerTests
    {
        [TestCase("2022-04-10","2","3")]
        [TestCase("2022-10-4", "3", "2")]
        [TestCase("2022-04-10", "1", "4")]
        [TestCase("2022-04-10", "1", "5")]
        public void BuildGetAllUkrlpsUpdatedSinceSoapRequest_ReturnsRequest(DateTime dateUpdated, string stakeholderId, string queryId)
        {
            var dateUpdatedSince = dateUpdated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");

            var serializer = new RoatpCourseManagement.Application.UkrlpData.UkrlpSoapSerializer();
            var actualRequest =
                serializer.BuildGetAllUkrlpsUpdatedSinceSoapRequest(dateUpdated, stakeholderId, queryId);

            var expectedRequest =
                $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ukr=\"http://ukrlp.co.uk.server.ws.v3\">\r\n  <soapenv:Header />\r\n  <soapenv:Body>\r\n    <ukr:ProviderQueryRequest>\r\n      <SelectionCriteria>\r\n        <StakeholderId>{stakeholderId}</StakeholderId>\r\n        <CriteriaCondition>OR</CriteriaCondition>\r\n        <ApprovedProvidersOnly>No</ApprovedProvidersOnly>\r\n        <ProviderStatus>A</ProviderStatus>\r\n        <ProviderUpdatedSince>{dateUpdatedSince}</ProviderUpdatedSince>\r\n      </SelectionCriteria>\r\n      <QueryId>{queryId}</QueryId>\r\n    </ukr:ProviderQueryRequest>\r\n  </soapenv:Body>\r\n</soapenv:Envelope>";

            Assert.AreEqual(expectedRequest,actualRequest);
        }

        [TestCase(12345678, 23456789, "2", "3")]
        [TestCase(12345678, 98765432, "3", "2")]
        [TestCase(12345678, 23456789, "1", "4")]
        [TestCase(87654321, 33333333, "1", "5")]
        public void BuildGetAllUkrlpsUpdatedSinceSoapRequest_ReturnsRequest(long ukprn1, long ukprn2, string stakeholderId, string queryId)
        {
            var ukprns = new List<long>
            {
                ukprn1,
                ukprn2
            };
            var serializer = new RoatpCourseManagement.Application.UkrlpData.UkrlpSoapSerializer();
            var actualRequest =
                serializer.BuildGetAllUkrlpsFromUkprnsSoapRequest(ukprns, stakeholderId, queryId);

            var expectedRequest =
                $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ukr=\"http://ukrlp.co.uk.server.ws.v3\">\r\n  <soapenv:Header />\r\n  <soapenv:Body>\r\n    <ukr:ProviderQueryRequest>\r\n      <SelectionCriteria>\r\n        <UnitedKingdomProviderReferenceNumberList>\r\n          <UnitedKingdomProviderReferenceNumber>{ukprn1}</UnitedKingdomProviderReferenceNumber>\r\n          <UnitedKingdomProviderReferenceNumber>{ukprn2}</UnitedKingdomProviderReferenceNumber>\r\n        </UnitedKingdomProviderReferenceNumberList>\r\n        <StakeholderId>{stakeholderId}</StakeholderId>\r\n        <CriteriaCondition>OR</CriteriaCondition>\r\n        <ApprovedProvidersOnly>No</ApprovedProvidersOnly>\r\n        <ProviderStatus>A</ProviderStatus>\r\n      </SelectionCriteria>\r\n      <QueryId>{queryId}</QueryId>\r\n    </ukr:ProviderQueryRequest>\r\n  </soapenv:Body>\r\n</soapenv:Envelope>";

            // the build jobs falls over with line endings
            expectedRequest = expectedRequest.Replace("\n", "").Replace("\r", "");
            actualRequest = actualRequest.Replace("\n", "").Replace("\r", "");

            Assert.AreEqual(expectedRequest, actualRequest);
        }

        [Test]
        public void DeserialiseMatchingProviderRecordsResponse_ReturnsMatchingRecords()
        {
            var soapXml = "<?xml version='1.0' encoding='UTF-8'?>\r\n<S:Envelope\r\n\txmlns:S=\"http://schemas.xmlsoap.org/soap/envelope/\">\r\n\t<S:Body>\r\n\t\t<ns0:ProviderQueryResponse\r\n\t\t\txmlns:ns0=\"http://ukrlp.co.uk.server.ws.v3\"\r\n\t\t\txmlns:ns2=\"http://www.govtalk.gov.uk/people/PersonDescriptives\">\r\n\t\t\t<MatchingProviderRecords>\r\n\t\t\t\t<UnitedKingdomProviderReferenceNumber>10000001</UnitedKingdomProviderReferenceNumber>\r\n\t\t\t\t<ProviderName>Company name 1</ProviderName>\r\n\t\t\t\t<ProviderStatus>Active</ProviderStatus>\r\n\t\t\t\t<ProviderContact>\r\n\t\t\t\t\t<ContactType>L</ContactType>\r\n\t\t\t\t\t<ContactAddress>\r\n\t\t\t\t\t\t<Address1>address line 1 for 1</Address1>\r\n\t\t\t\t\t\t<Address2>address line 2 for 1</Address2>\r\n\t\t\t\t\t\t<Address3>address line 3 for 1</Address3>\r\n\t\t\t\t\t\t<Address4>address line 4 for 1</Address4>\r\n\t\t\t\t\t\t<Town>Town 1</Town>\r\n\t\t\t\t\t\t<PostCode>Postcode 1</PostCode>\r\n\t\t\t\t\t</ContactAddress>\r\n\t\t\t\t\t<ContactPersonalDetails/>\r\n\t\t\t\t\t<ContactTelephone1>12345678</ContactTelephone1>\r\n\t\t\t\t\t<ContactFax>8888888888</ContactFax>\r\n\t\t\t\t\t<LastUpdated>2019-01-09</LastUpdated>\r\n\t\t\t\t</ProviderContact>\r\n\t\t\t\t<ProviderVerificationDate>2022-09-15</ProviderVerificationDate>\r\n\t\t\t\t<ProviderAliases/>\r\n\t\t\t\t<VerificationDetails>\r\n\t\t\t\t\t<PrimaryVerificationSource>true</PrimaryVerificationSource>\r\n\t\t\t\t\t<VerificationAuthority>Companies House</VerificationAuthority>\r\n\t\t\t\t\t<VerificationID>11111111</VerificationID>\r\n\t\t\t\t</VerificationDetails>\r\n\t\t\t</MatchingProviderRecords>\r\n\t\t\t<MatchingProviderRecords>\r\n\t\t\t\t<UnitedKingdomProviderReferenceNumber>10000002</UnitedKingdomProviderReferenceNumber>\r\n\t\t\t\t<ProviderName>Company name 2</ProviderName>\r\n\t\t\t\t<ProviderStatus>Active</ProviderStatus>\r\n\t\t\t\t<ProviderContact>\r\n\t\t\t\t\t<ContactType>L</ContactType>\r\n\t\t\t\t\t<ContactAddress>\r\n\t\t\t\t\t\t<Address1>address line 1 for 2</Address1>\r\n\t\t\t\t\t\t<Address2>address line 2 for 2</Address2>\r\n\t\t\t\t\t\t<Address3>address line 3 for 2</Address3>\r\n\t\t\t\t\t\t<Address4>address line 4 for 2</Address4>\r\n\t\t\t\t\t\t<Town>Town 2</Town>\r\n\t\t\t\t\t\t<PostCode>Postcode 2</PostCode>\r\n\t\t\t\t\t</ContactAddress>\r\n\t\t\t\t\t<ContactPersonalDetails/>\r\n\t\t\t\t\t<ContactTelephone1>12345678</ContactTelephone1>\r\n\t\t\t\t\t<ContactFax>8888888888</ContactFax>\r\n\t\t\t\t\t<LastUpdated>2019-01-09</LastUpdated>\r\n\t\t\t\t</ProviderContact>\r\n\t\t\t\t<ProviderVerificationDate>2022-09-15</ProviderVerificationDate>\r\n\t\t\t\t<ProviderAliases/>\r\n\t\t\t\t<VerificationDetails>\r\n\t\t\t\t\t<PrimaryVerificationSource>true</PrimaryVerificationSource>\r\n\t\t\t\t\t<VerificationAuthority>Companies House</VerificationAuthority>\r\n\t\t\t\t\t<VerificationID>11111111</VerificationID>\r\n\t\t\t\t</VerificationDetails>\r\n\t\t\t</MatchingProviderRecords>\r\n\t\t\t<QueryId>2</QueryId>\r\n\t\t\t<StakeholderId>2</StakeholderId>\r\n\t\t</ns0:ProviderQueryResponse>\r\n\t</S:Body>\r\n</S:Envelope>";
            var serializer = new RoatpCourseManagement.Application.UkrlpData.UkrlpSoapSerializer();
            var response = serializer.DeserialiseMatchingProviderRecordsResponse(soapXml);
            Assert.AreEqual(response.Count,2);

            var company1 = response.First(x => x.UnitedKingdomProviderReferenceNumber == "10000001");
            Assert.AreEqual("Company name 1", company1.ProviderName);
            Assert.AreEqual("address line 1 for 1", company1.ProviderContacts[0].ContactAddress.Address1);
            Assert.AreEqual("address line 2 for 1", company1.ProviderContacts[0].ContactAddress.Address2); 
            Assert.AreEqual("address line 3 for 1", company1.ProviderContacts[0].ContactAddress.Address3); 
            Assert.AreEqual("address line 4 for 1", company1.ProviderContacts[0].ContactAddress.Address4); 
            Assert.AreEqual("Town 1", company1.ProviderContacts[0].ContactAddress.Town); 
            Assert.AreEqual("Postcode 1", company1.ProviderContacts[0].ContactAddress.PostCode);
            Assert.IsNotEmpty(company1.VerificationDetails[0].VerificationAuthority);
        }
    }
}
