using System;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Requests
{
    public class PostVacancyRequest : IPostApiRequest
    {
        private readonly Guid _id;

        public PostVacancyRequest(Guid id, PostVacancyRequestData postVacancyRequestData)
        {
            _id = id;
            Data = postVacancyRequestData;
        }

        public string PostUrl => $"api/Vacancies/{_id}";
        public object Data { get; set; }
    }

    
}