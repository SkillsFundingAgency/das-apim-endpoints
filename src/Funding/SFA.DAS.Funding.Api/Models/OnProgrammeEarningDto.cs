using SFA.DAS.Funding.Models;

namespace SFA.DAS.Funding.Api.Models
{
    public class OnProgrammeEarningDto
    {
        public short AcademicYear { get; set; }
        public byte DeliveryPeriod { get; set; }
        public decimal Amount { get; set; }

        public static implicit operator OnProgrammeEarningDto(OnProgrammeEarning source)
        {
            return new OnProgrammeEarningDto
            {
                AcademicYear = source.AcademicYear,
                DeliveryPeriod = source.DeliveryPeriod,
                Amount = source.Amount
            };
        }
    }
}
