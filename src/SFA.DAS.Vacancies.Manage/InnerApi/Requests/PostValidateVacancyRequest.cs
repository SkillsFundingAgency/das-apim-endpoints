using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Requests
{
    public class PostValidateVacancyRequest : IPostApiRequest
    {
        private readonly Guid _id;
        private readonly long? _ukprn;
        private readonly string _email;

        public PostValidateVacancyRequest(Guid id, PostVacancyRequestData postVacancyRequestData)
        {
            _id = id;
            _ukprn = postVacancyRequestData.User.Ukprn;
            _email = postVacancyRequestData.User.Email;
            Data = postVacancyRequestData;
        }
        
        public string PostUrl => $"api/vacancies/{_id}/validate?ukprn={_ukprn}&userEmail={_email}";
        public object Data { get; set; }
    }
}