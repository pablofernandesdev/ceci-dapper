﻿using Bogus;
using CeciDapper.Domain.DTO.Commons;
using System.Net;

namespace CeciDapper.Test.Fakers.Commons
{
    public static class ResultResponseFaker
    {
        public static Faker<ResultResponse> ResultResponse(HttpStatusCode httpStatusCode)
        {
            return new Faker<ResultResponse>()
                .RuleFor(p => p.Message, p => p.Random.Words(3))
                .RuleFor(p => p.Details, p => p.Random.Words(3))
                .RuleFor(p => p.StatusCode, httpStatusCode);
        }

        public static Faker<ResultResponse<TData>> ResultResponseData<TData>(TData data, HttpStatusCode httpStatusCode)
        {
            return new Faker<ResultResponse<TData>>()
                .CustomInstantiator(p => new ResultResponse<TData>
                {
                    Data = data,
                    StatusCode = httpStatusCode
                });
        }

        /*public static Faker<ResultResponse> ResultResponseBadRequestFaker()
        {
            return new Faker<ResultResponse>()
                .RuleFor(p => p.Message, p => p.Random.Words(3))
                .RuleFor(p => p.Details, p => p.Random.Words(3))
                .RuleFor(p => p.StatusCode, p => System.Net.HttpStatusCode.BadRequest);
        }*/
    }
}
