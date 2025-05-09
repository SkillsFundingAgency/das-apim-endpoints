using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Vacancies.Api
{
    public class SearchVacancyRequestV2 : SearchVacancyRequest
    {
        /// <summary>
        /// The search term to use when searching for vacancies. This can be a keyword, location, or other criteria. #TODO: Description will be updated in the future.
        /// </summary>
        [FromQuery]
        public string EmployerName { get; set; }
    }
}