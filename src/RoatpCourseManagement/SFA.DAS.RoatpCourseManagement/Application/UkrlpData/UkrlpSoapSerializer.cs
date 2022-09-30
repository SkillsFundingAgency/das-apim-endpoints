using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace SFA.DAS.RoatpCourseManagement.Application.UkrlpData
{
    public class UkrlpSoapSerializer : IUkrlpSoapSerializer
    {
        public string BuildGetAllUkrlpsUpdatedSinceSoapRequest(DateTime providerUpdatedSince, string stakeholderId, string queryId)
        {
            var selectionCriteriaElement = new XElement("SelectionCriteria",
                new XElement("StakeholderId", stakeholderId),
                new XElement("CriteriaCondition", new XText("OR")),
                new XElement("ApprovedProvidersOnly", "No"),
                new XElement("ProviderStatus", "A"),
                new XElement("ProviderUpdatedSince", providerUpdatedSince)

            );

            return BuildSoapRequest(queryId, selectionCriteriaElement);
        }

        public string BuildGetAllUkrlpsFromUkprnsSoapRequest(List<long> ukprns, string stakeholderId, string queryId)
        {
            var selectionCriteriaElement = new XElement("SelectionCriteria",
                new XElement("UnitedKingdomProviderReferenceNumberList", ukprns
                    .Select(ukprn => new XElement("UnitedKingdomProviderReferenceNumber", new XText(ukprn.ToString())))
                    .ToArray()),
                 new XElement("StakeholderId", stakeholderId),
                 new XElement("CriteriaCondition", new XText("OR")),
                 new XElement("ApprovedProvidersOnly", "No"),
                 new XElement("ProviderStatus", "A")
                    );

            return BuildSoapRequest(queryId, selectionCriteriaElement);
        }

        public string BuildUkrlpSoapRequest(long ukprn, string stakeholderId, string queryId)
        {
            var selectionCriteriaElement = new XElement("SelectionCriteria",
                new XElement("UnitedKingdomProviderReferenceNumberList",
                    new XElement("UnitedKingdomProviderReferenceNumber", new XText(ukprn.ToString()))),
                new XElement("CriteriaCondition", new XText("OR")),
                new XElement("StakeholderId", stakeholderId),
                new XElement("ApprovedProvidersOnly", "No"),
                new XElement("ProviderStatus", "A")
            );

            return BuildSoapRequest(queryId, selectionCriteriaElement);
        }

        private static string BuildSoapRequest(string queryId, XElement selectionCriteriaElement)
        {
            var queryIdElement = new XElement(XName.Get("QueryId"), new XText(queryId));

            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace ukr = "http://ukrlp.co.uk.server.ws.v3";

            var providerQueryRequest =
                new XElement(ukr + "ProviderQueryRequest", selectionCriteriaElement, queryIdElement);

            var soapBodyElement = new XElement(soapenv + "Body", providerQueryRequest);

            var soapHeaderElement = new XElement(soapenv + "Header");

            var soapEnvelope = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", soapenv.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "ukr", ukr.NamespaceName),
                soapHeaderElement, soapBodyElement);

            return soapEnvelope.ToString();
        }

        public List<MatchingProviderRecords> DeserialiseMatchingProviderRecordsResponse(string soapXml)
        {
            var soapDocument = XDocument.Parse(soapXml);
            var queryResponses = soapDocument.XPathSelectElements("//MatchingProviderRecords");

            if (queryResponses == null)
            {
                return null;
            }

            var matches = new List<MatchingProviderRecords>();

            foreach (var queryResponse in queryResponses)
            {
                // UKRLP SOAP service doesn't return contacts and verification details arrays in a 
                // wrapping tag, so can't serialize using XmlArray
                var matchingRecordsSerializer = new XmlSerializer(typeof(MatchingProviderRecords));
                var contactSerializer = new XmlSerializer(typeof(ProviderContactStructure));
                var verificationDetailsSerializer = new XmlSerializer(typeof(VerificationDetailsStructure));

                MatchingProviderRecords matchingProviderRecords =
                    (MatchingProviderRecords)matchingRecordsSerializer.Deserialize(queryResponse.CreateReader());

                var contactElements = queryResponse.Descendants(XName.Get("ProviderContact"));
                if (contactElements != null)
                {
                    matchingProviderRecords.ProviderContacts = new List<ProviderContactStructure>();
                    foreach (var contactElement in contactElements)
                    {
                        var contact =
                            (ProviderContactStructure)contactSerializer.Deserialize(contactElement.CreateReader());
                        matchingProviderRecords.ProviderContacts.Add(contact);
                    }
                }

                var verificationElements = queryResponse.Descendants(XName.Get("VerificationDetails"));
                if (verificationElements != null)
                {
                    matchingProviderRecords.VerificationDetails = new List<VerificationDetailsStructure>();
                    foreach (var verificationElement in verificationElements)
                    {
                        var verification =
                            (VerificationDetailsStructure)verificationDetailsSerializer.Deserialize(verificationElement
                                .CreateReader());
                        matchingProviderRecords.VerificationDetails.Add(verification);
                    }
                }
                matches.Add(matchingProviderRecords);
            }

            return matches;
        }
    }
}
