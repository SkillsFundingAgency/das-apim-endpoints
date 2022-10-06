using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SFA.DAS.RoatpCourseManagement.Application.UkrlpData
{
    [Serializable]
    public class MatchingProviderRecords
    {
        [XmlElement]
        public string UnitedKingdomProviderReferenceNumber { get; set; }

        [XmlElement]
        public string ProviderName { get; set; }

        [XmlArray(ElementName = "ProviderContact")]
        public List<ProviderContactStructure> ProviderContacts { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "ProviderContact")]
    public class ProviderContactStructure
    {
        [XmlElement]
        public string ContactType { get; set; }

        [XmlElement]
        public ProviderContactAddress ContactAddress { get; set; }
    }

    [Serializable]
    public class ProviderContactAddress
    {
        [XmlElement]
        public string Address1 { get; set; }

        [XmlElement]
        public string Address2 { get; set; }

        [XmlElement]
        public string Address3 { get; set; }

        [XmlElement]
        public string Address4 { get; set; }

        [XmlElement]
        public string Town { get; set; }

        [XmlElement]
        public string PostCode { get; set; }
    }
}
