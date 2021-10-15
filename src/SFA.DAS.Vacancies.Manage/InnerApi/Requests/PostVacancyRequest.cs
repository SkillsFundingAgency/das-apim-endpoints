using System;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Requests
{
    public class PostVacancyRequest : IPostApiRequest
    {
        private readonly Guid _id;
        private readonly long? _ukprn;
        private readonly string _email;

        public PostVacancyRequest(Guid id, PostVacancyRequestData postVacancyRequestData)
        {
            _id = id;
            _ukprn = postVacancyRequestData.User.Ukprn;
            _email = postVacancyRequestData.User.Email;
            Data = postVacancyRequestData;
        }

        public string PostUrl => $"api/Vacancies/{_id}?ukprn={_ukprn}&userEmail={_email}";
        public object Data { get; set; }
    }

    
}