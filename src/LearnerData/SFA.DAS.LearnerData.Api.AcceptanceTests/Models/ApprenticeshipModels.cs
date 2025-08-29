namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Models;

public class ApprenticeshipModel
{
    public ApprenticeshipModel()
    {
        PriceEpisodes = new List<PriceEpisodeModel>();
        AdditionalPayments = new List<AdditionalPaymentModel>();
        Instalments = new List<InstalmentModel>();
    }

    public List<PriceEpisodeModel> PriceEpisodes { get; set; }
    public List<AdditionalPaymentModel> AdditionalPayments { get; set; }
    public List<InstalmentModel> Instalments { get; set; }
}

public class PriceEpisodeModel
{
    public int PriceEpisodeId { get; set; }
    public Guid Key { get; set; } = Guid.NewGuid();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class InstalmentModel
{
    public int PriceEpisodeId { get; set; }
    public short AcademicYear { get; set; }
    public byte DeliveryPeriod { get; set; }
    public decimal Amount { get; set; }
    public string InstalmentType { get; set; }
}

public class AdditionalPaymentModel
{
    public string Type { get; set; }
    public short AcademicYear { get; set; }
    public byte DeliveryPeriod { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
}

public class PeriodisedValuesModel
{
    public int Episode { get; set; }
    public string Attribute { get; set; }
    public int Period { get; set; }
    public decimal Value { get; set; }


}

