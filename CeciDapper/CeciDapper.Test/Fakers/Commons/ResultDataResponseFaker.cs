using Bogus;
using CeciDapper.Domain.DTO.Commons;
using System.Net;

namespace CeciDapper.Test.Fakers.Commons
{
    public static class ResultDataResponseFaker
    {
        public static Faker<ResultDataResponse<TData>> ResultDataResponse<TData>(TData data, HttpStatusCode httpStatusCode)
        {
            return new Faker<ResultDataResponse<TData>>()
                .CustomInstantiator(p => new ResultDataResponse<TData>
                {
                    Data = data,
                    StatusCode = httpStatusCode,
                    TotalItems = p.Random.Int(),
                    TotalPages = p.Random.Int()
                });
        }
    }
}
