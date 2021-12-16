using SFA.DAS.Roatp.Domain.Models;

namespace SFA.DAS.Roatp.Application.Charities.Queries
{
    public class GetCharityResult
    {
        public Charity Charity { get; }
        public GetCharityResult(Charity charity)
        {
            Charity = charity;
        }
    }
}
