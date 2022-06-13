using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetTraineeshipVacancyAddressItem
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }

        public static implicit operator GetTraineeshipVacancyAddressItem(GetTraineeshipVacanciesListItem source)
        {
            if (source.Address == null)
            {
                return new GetTraineeshipVacancyAddressItem();
            }

            return new GetTraineeshipVacancyAddressItem
            {
                AddressLine1 = source.Address.AddressLine1,
                AddressLine2 = source.Address.AddressLine2,
                AddressLine3 = source.Address.AddressLine3,
                AddressLine4 = source.Address.AddressLine4,
                Postcode = source.Address.Postcode,
            };
        }
    }
}