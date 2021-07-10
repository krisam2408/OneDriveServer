using Gluttony.DataTransfer;
using System;
using System.Threading.Tasks;

namespace Gluttony.Abstracts
{
    public interface IRequestClient
    {
        string BaseAddress { get; init; }
        Task<T> SendAsync<T>(IRequest<T> request) where T : Response, new();
    }
}
