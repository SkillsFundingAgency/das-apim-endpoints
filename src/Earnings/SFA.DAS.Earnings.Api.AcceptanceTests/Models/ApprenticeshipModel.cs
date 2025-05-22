using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Earnings.Api.AcceptanceTests.Models
{
    public class ApprenticeshipModel
    {
        public int Age { get; set; }

        public List<PriceEpisode> PriceEpisodes { get; set; }
        public List<AdditionalPayment> AdditionalPayments { get; set; }
        public List<Instalment> Instalments { get; set; }
    }

    public class PriceEpisode
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class Instalment
    {
        public short AcademicYear { get; set; }
        public byte DeliveryPeriod { get; set; }
        public decimal Amount { get; set; }
    }

    public class AdditionalPayment
    {
        public string Type { get; set; }
        public short AcademicYear { get; set; }
        public byte DeliveryPeriod { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
    }

    public class PriceEpisodePeriodisedValues
    {
        public int Episode { get; set; }
        public string Attribute { get; set; }
        public int AcademicYear { get; set; }
        public int Period { get; set; }
        public decimal Value { get; set; }

        
    }
}
