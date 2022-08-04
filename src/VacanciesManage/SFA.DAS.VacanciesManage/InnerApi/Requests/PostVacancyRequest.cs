using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.VacanciesManage.InnerApi.Requests
{
    public class PostVacancyRequest : IPostApiRequest
    {
        private readonly Guid _id;
        private readonly long? _ukprn;
        private readonly string _email;

        public PostVacancyRequest(Guid id, int ukprn, string email, PostVacancyRequestData postVacancyRequestData)
        {
            _id = id;
            _ukprn = ukprn;
            _email = email;
            Data = postVacancyRequestData;
        }

        public string PostUrl => $"api/Vacancies/{_id}?ukprn={_ukprn}&userEmail={_email}";
        public object Data { get; set; }
    }
}