﻿using System;
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

        public List<Provider> DeserialiseMatchingProviderRecordsResponse(string soapXml)
        {
            var soapDocument = XDocument.Parse(soapXml);
            var queryResponses = soapDocument.XPathSelectElements("//MatchingProviderRecords");

            var matches = new List<Provider>();

            foreach (var queryResponse in queryResponses)
            {
                // UKRLP SOAP service doesn't return contacts arrays in a 
                // wrapping tag, so can't serialize using XmlArray
                var matchingRecordsSerializer = new XmlSerializer(typeof(Provider));
                var contactSerializer = new XmlSerializer(typeof(ProviderContact));
           
                Provider provider =
                    (Provider)matchingRecordsSerializer.Deserialize(queryResponse.CreateReader());

                var contactElements = queryResponse.Descendants(XName.Get("ProviderContact"));
                if (contactElements != null)
                {
                    provider.ProviderContacts = new List<ProviderContact>();
                    foreach (var contactElement in contactElements)
                    {
                        var contact =
                            (ProviderContact)contactSerializer.Deserialize(contactElement.CreateReader());
                        provider.ProviderContacts.Add(contact);
                    }
                }

                matches.Add(provider);
            }

            return matches;
        }
    }
}
