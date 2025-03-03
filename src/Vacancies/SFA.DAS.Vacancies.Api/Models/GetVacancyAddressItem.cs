using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public class GetVacancyAddressItem
    {
        /// <summary>
        /// First line of the address where the apprentice will work.
        /// </summary>
        public string AddressLine1 { get; set; }
        /// <summary>
        /// Second line of the address where the apprentice will work.
        /// </summary>
        public string AddressLine2 { get; set; }
        /// <summary>
        /// Third line of the address where the apprentice will work.
        /// </summary>
        public string AddressLine3 { get; set; }
        /// <summary>
        /// Fourth line of the address where the apprentice will work.
        /// </summary>
        public string AddressLine4 { get; set; }
        /// <summary>
        /// Postcode of the address where the apprentice will work.
        /// </summary>
        public string Postcode { get; set; }

        public static implicit operator GetVacancyAddressItem(GetVacanciesListItem source)
        {
            if (source.Address == null)
            {
                return new GetVacancyAddressItem();
            }

            return new GetVacancyAddressItem
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