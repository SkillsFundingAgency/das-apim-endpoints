using System;
using SFA.DAS.SharedOuterApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IApiClient<T> : IGetApiClient<T>
    {
        Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request);
        Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request);
        [Obsolete("Use PostWithResponseCode")]
        Task<TResponse> Post<TResponse>(IPostApiRequest request);
        [Obsolete("Use PostWithResponseCode")]
        Task Post<TData>(IPostApiRequest<TData> request);
        Task Delete(IDeleteApiRequest request);
        Task Patch<TData>(IPatchApiRequest<TData> request);
        Task Put(IPutApiRequest request);
        Task Put<TData>(IPutApiRequest<TData> request);
        Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true);
        Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request);
        /// <summary>
        /// Sends a PUT request to an API endpoint and returns a response code.
        /// </summary>
        /// <typeparam name="TResponse">If the API returns no data use NullResponse type.</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request);
    }
}
